using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nemesys.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nemesys.Models.Repositories
{
    public class SqlNemesysRepository : INemesysRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<SqlNemesysRepository> _logger;

        public SqlNemesysRepository(AppDbContext appDbContext, ILogger<SqlNemesysRepository> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        //Creates investigation from instance
        public Investigation CreateInvestigation(Investigation investigation)
        {
            try
            {
                _appDbContext.Investigations.Add(investigation);
                _appDbContext.SaveChanges();

                return investigation;
            } 
            catch (DbUpdateException)
            {
                _logger.LogError("Failed to create investigation by user " + investigation.UserId);
                return null;
            }
        }

        public Report CreateReport(Report report)
        {
            try
            {
                _appDbContext.Reports.Add(report);
                _appDbContext.SaveChanges();

                User user = _appDbContext.Users.Find(report.UserId);
                user.NumberOfReports++;

                _appDbContext.Entry(user).State = EntityState.Modified;
                _appDbContext.SaveChanges();

                return report;
            } 
            catch (DbUpdateException)
            {
                _logger.LogError($"Failed to create report by user {0}", report.UserId);
                return null;
            }
        }

        public bool StarReport(string userId, int reportId)
        {
            try
            {
                Report report = _appDbContext.Reports.Include(r => r.Author)
                    .SingleOrDefault(r => r.Id == reportId);

                if (report != null)
                {
                    User author = report.Author;

                    var record = _appDbContext.StarRecords
                        .SingleOrDefault(record => record.UserId == userId && record.ReportId == reportId);

                    if (record == null)
                    {
                        _appDbContext.StarRecords.Add(new StarRecord
                        {
                            UserId = userId,
                            ReportId = reportId,
                        });

                        _appDbContext.SaveChanges();

                        report.NumberOfStars++;
                        author.NumberOfStars++;
                    }
                    else
                    {
                        _appDbContext.StarRecords.Remove(record);
                        _appDbContext.SaveChanges();

                        report.NumberOfStars--;
                        author.NumberOfStars--;
                    }

                    _appDbContext.Entry(report).State = EntityState.Modified;
                    _appDbContext.Entry(author).State = EntityState.Modified;
                    _appDbContext.SaveChanges();
                }
                else
                {
                    return false;
                }
            }
            catch (DbUpdateException)
            {
                _logger.LogError($"Failed to star report (ID: {0}) by user {1}", reportId, userId);
                return false;
            }

            return true;
        }
        
        public HazardType GetHazardTypeById(int hazardId)
        {
            return _appDbContext.HazardTypes
                .Include(h => h.Reports)
                .SingleOrDefault(hazardType => hazardType.Id == hazardId);
        }

        public IEnumerable<HazardType> GetHazardTypes()
        {
            return _appDbContext.HazardTypes.Include(h => h.Reports);
        }

        public Investigation GetInvestigationById(int investigationId)
        {
            return _appDbContext.Investigations
                .Include(i => i.Investigator)
                .Include(i => i.Report)
                    .ThenInclude(r => r.Status)
                .SingleOrDefault(i => i.InvestigationId == investigationId);
        }

        public Report GetReportById(int reportId)
        {
            return _appDbContext.Reports
                .Include(r => r.Author)
                .Include(r => r.HazardType)
                .Include(r => r.Status)
                .SingleOrDefault(r => r.Id == reportId);
        }

        public IEnumerable<Report> GetAllReports()
        {
            return _appDbContext.Reports
                .Include(r => r.Author)
                .Include(r => r.HazardType)
                .Include(r => r.Status);
        }

        public IEnumerable<Report> GetAllReportsWithStatus(int id)
        {
            var statusId = GetReportStatusById(id);
            if (statusId == null)
                return null;
            return statusId.Reports;
        }

        public ReportStatus GetReportStatusById(int id)
        {
            return _appDbContext.ReportStatuses
                .Include(s => s.Reports)
                .SingleOrDefault(status => status.Id == id);
        }

        public IEnumerable<ReportStatus> GetReportStatuses()
        {
            return _appDbContext.ReportStatuses
                .Include(s => s.Reports);
        }

        public IEnumerable<Report> GetStarredUserReports(string userId)
        {
            return _appDbContext.StarRecords
                .Include(r => r.User)
                .Include(r => r.Report)
                .Where(record => record.UserId == userId)
                .Select(record => record.Report);
        }

        public IEnumerable<User> GetUsersWhoStarredReport(int reportId)
        {
            return _appDbContext.StarRecords
                .Include(r => r.Report)
                .Include(r => r.User)
                .Where(record => record.ReportId == reportId)
                .Select(record => record.User);
        }

        public User GetUserById(string userId)
        {
            return GetUsers()
                .SingleOrDefault(u => u.Id == userId);
        }

        public IEnumerable<User> GetUsers()
        {
            return _appDbContext.Users
                .AsSplitQuery()
                .Include(u => u.Reports)
                    .ThenInclude(r => r.HazardType)
                .Include(u => u.Reports)
                    .ThenInclude(r => r.Status)
                .Include(u => u.StarredReports);
        }

        public IEnumerable<User> GetTopUsers(int amount)
        {
            return GetUsers().OrderByDescending(user => user.NumberOfStars)
                .ThenBy(user => user.Alias)
                .Take(amount);
        }

        public bool UpdateInvestigation(Investigation updatedInvestigation)
        {
            var existingInvestigation = _appDbContext.Investigations.Find(updatedInvestigation.InvestigationId);

            if (existingInvestigation != null)
            {
                try
                {
                    existingInvestigation.Description = updatedInvestigation.Description;
                    existingInvestigation.DateOfAction = updatedInvestigation.DateOfAction;
                    
                    _appDbContext.Entry(existingInvestigation).State = EntityState.Modified;
                    _appDbContext.SaveChanges();
                } 
                catch (DbUpdateException)
                {
                    _logger.LogError($"Failed to update investigation (ID: {0})", updatedInvestigation.InvestigationId);
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateReport(Report updatedReport)
        {
            var existingReport = _appDbContext.Reports.Find(updatedReport.Id);

            if (existingReport != null)
            {
                try
                {
                    existingReport.DateTimeOfHazard = updatedReport.DateTimeOfHazard;
                    existingReport.HazardTypeId = updatedReport.HazardTypeId;
                    existingReport.Description = updatedReport.Description;
                    existingReport.Latitude = updatedReport.Latitude;
                    existingReport.Longitude = updatedReport.Longitude;
                    existingReport.StatusId = updatedReport.StatusId;
                    existingReport.Photo = updatedReport.Photo;
                    existingReport.DateOfUpdate = DateTime.UtcNow;

                    _appDbContext.Entry(existingReport).State = EntityState.Modified;
                    _appDbContext.SaveChanges();
                } 
                catch (DbUpdateException)
                {
                    _logger.LogError($"Failed to update report (ID: {0})", updatedReport.Id);
                    return false;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteReport(int reportId)
        {
            var report = _appDbContext.Reports.Find(reportId);

            if (report != null)
            {
                _appDbContext.Reports.Remove(report);
                _appDbContext.SaveChanges();
            }
            else
            {
                _logger.LogError($"Failed to delete report (ID: {0})", reportId);
                return false;
            }

            return true;
        }

    }
}
