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
        private readonly ILogger _logger;

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
                throw;
            }
        }

        //Creates report from instance
        public Report CreateReport(Report report)
        {
            try
            {
                _appDbContext.Reports.Add(report);
                _appDbContext.SaveChanges();

                //When creating report, user NumberOfReports counter needs to be updated
                User user = _appDbContext.Users.Find(report.UserId);
                user.NumberOfReports++;

                _appDbContext.Entry(user).State = EntityState.Modified;
                _appDbContext.SaveChanges();

                return report;
            } 
            catch (DbUpdateException)
            {
                _logger.LogError($"Failed to create report by user {0}", report.UserId);
                throw;
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
                        //If the record is unstarred, create a new database entry
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
                        //If the record is starred, remove the entry from the database
                        _appDbContext.StarRecords.Remove(record);
                        _appDbContext.SaveChanges();

                        report.NumberOfStars--;
                        author.NumberOfStars--;
                    }

                    //Author and report counters are updated accordingly
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
                throw;
            }

            return true;
        }
        
        public HazardType GetHazardTypeById(int hazardId)
        {
            try
            {
                return _appDbContext.HazardTypes
                    .Include(h => h.Reports)
                    .SingleOrDefault(hazardType => hazardType.Id == hazardId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public IEnumerable<HazardType> GetHazardTypes()
        {
            try
            {
                return _appDbContext.HazardTypes.Include(h => h.Reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public Investigation GetInvestigationById(int investigationId)
        {
            try
            {
                return _appDbContext.Investigations
                    .Include(i => i.Investigator)
                    .Include(i => i.Report)
                        .ThenInclude(r => r.Status)
                    .SingleOrDefault(i => i.InvestigationId == investigationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public Report GetReportById(int reportId)
        {
            try
            {
                return _appDbContext.Reports
                    .Include(r => r.Author)
                    .Include(r => r.HazardType)
                    .Include(r => r.Status)
                    .SingleOrDefault(r => r.Id == reportId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public IEnumerable<Report> GetAllReports()
        {
            try
            {
                return _appDbContext.Reports
                    .Include(r => r.Author)
                    .Include(r => r.HazardType)
                    .Include(r => r.Status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public IEnumerable<Report> GetAllReportsWithStatus(int id)
        {
            try
            {
                var statusId = GetReportStatusById(id);
                if (statusId == null)
                    return null;
                return statusId.Reports;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public ReportStatus GetReportStatusById(int id)
        {
            try
            {
                return _appDbContext.ReportStatuses
                    .Include(s => s.Reports)
                    .SingleOrDefault(status => status.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public IEnumerable<ReportStatus> GetReportStatuses()
        {
            try
            {
                return _appDbContext.ReportStatuses
                    .Include(s => s.Reports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public IEnumerable<Report> GetStarredUserReports(string userId)
        {
            try
            {
                return _appDbContext.StarRecords
                    .Include(r => r.User)
                    .Include(r => r.Report)
                    .Where(record => record.UserId == userId)
                    .Select(record => record.Report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public IEnumerable<User> GetUsersWhoStarredReport(int reportId)
        {
            try
            {
                return _appDbContext.StarRecords
                    .Include(r => r.Report)
                    .Include(r => r.User)
                    .Where(record => record.ReportId == reportId)
                    .Select(record => record.User);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public User GetUserById(string userId)
        {
            try
            {
                return GetUsers()
                    .SingleOrDefault(u => u.Id == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public IEnumerable<User> GetUsers()
        {
            try {
                return _appDbContext.Users
                    .AsSplitQuery()
                    .Include(u => u.Reports)
                        .ThenInclude(r => r.HazardType)
                    .Include(u => u.Reports)
                        .ThenInclude(r => r.Status)
                    .Include(u => u.StarredReports);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public IEnumerable<User> GetTopUsers(int amount)
        {
            try
            {
                return GetUsers().OrderByDescending(user => user.NumberOfStars)
                    .ThenBy(user => user.Alias)
                    .Take(amount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
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
                    throw;
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
                    throw;
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
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
