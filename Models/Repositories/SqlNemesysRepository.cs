using Microsoft.EntityFrameworkCore;
using Nemesys.Models.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Nemesys.Models.Repositories
{
    public class SqlNemesysRepository : INemesysRepository
    {
        private readonly AppDbContext _appDbContext;

        public SqlNemesysRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Investigation CreateInvestigation(Investigation investigation)
        {
            _appDbContext.Investigations.Add(investigation);
            _appDbContext.SaveChanges();
            return investigation;
        }

        public Report CreateReport(Report report)
        {
            _appDbContext.Reports.Add(report);
            _appDbContext.SaveChanges();
            return report;
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
                return false;
            }

            return true;
        }

        public bool DeleteUser(string userId)
        {
            var user = _appDbContext.Users.Find(userId);

            if (user != null)
            {
                _appDbContext.Users.Remove(user);
                _appDbContext.SaveChanges();
            } else
            {
                return false;
            }

            return true;
        }

        public IEnumerable<Investigation> GetAllInvestigations()
        {
            return _appDbContext.Investigations.Include(i => i.Report);
        }

        public IEnumerable<Report> GetAllReports()
        {
            return _appDbContext.Reports
                .Include(r => r.Author)
                .Include(r => r.HazardType)
                .Include(r => r.Status);
        }

        public IEnumerable<Report> GetAllReportsWithStatus(int statusId)
        {
            return GetReportStatusById(statusId).Reports;
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

        public ReportStatus GetReportStatusById(int statusId)
        {
            return _appDbContext.ReportStatuses
                .Include(s => s.Reports)
                .SingleOrDefault(status => status.Id == statusId);
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

        public IEnumerable<User> GetTopUsers(int amount)
        {
            return GetUsers().OrderBy(user => user.NumberOfReports).Take(amount);
        }

        public User GetUserById(string userId)
        {
            return GetUsers()
                .SingleOrDefault(u => u.Id == userId);
        }

        public IEnumerable<User> GetUsers()
        {
            return _appDbContext.Users
                .Include(u => u.Reports)
                    .ThenInclude(r => r.HazardType)
                .Include(u => u.Reports)
                    .ThenInclude(r => r.Status)
                .Include(u => u.StarredReports);
        }

        public IEnumerable<User> GetUsersWhoStarredReport(int reportId)
        {
            return _appDbContext.StarRecords
                .Include(r => r.Report)
                .Include(r => r.User)
                .Where(record => record.ReportId == reportId)
                .Select(record => record.User);
        }

        public bool RemoveInvestigation(int investigationId)
        {
            var investigation = _appDbContext.Investigations.SingleOrDefault(investigation => investigation.InvestigationId == investigationId);

            if (investigation != null)
            {
                _appDbContext.Investigations.Remove(investigation);
                _appDbContext.SaveChanges();
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool StarReport(string userId, int reportId)
        {
            var record = _appDbContext.StarRecords
                .SingleOrDefault(record => record.UserId == userId && record.ReportId == reportId);

            if (record != null)
            {
                User user = (User)_appDbContext.Users.Find(userId);
                Report report = _appDbContext.Reports.Find(reportId);

                _appDbContext.StarRecords.Add(new StarRecord
                {
                    UserId = userId,
                    User = user,
                    ReportId = reportId,
                    Report = report
                });
            } else
            {
                _appDbContext.StarRecords.Remove(record);
            }

            _appDbContext.SaveChanges();

            return true;
        }

        public bool UpdateInvestigation(Investigation updatedInvestigation)
        {
            var existingInvestigation = _appDbContext.Investigations.SingleOrDefault(p => p.InvestigationId == updatedInvestigation.InvestigationId);

            if (existingInvestigation != null)
            {
                existingInvestigation.Description = updatedInvestigation.Description;
                existingInvestigation.DateOfAction = updatedInvestigation.DateOfAction;

                _appDbContext.Entry(existingInvestigation).State = EntityState.Modified;
                _appDbContext.SaveChanges();
            }

            return true;
        }

        public bool UpdateReport(Report updatedReport)
        {
            var existingReport = _appDbContext.Reports.SingleOrDefault(p => p.Id == updatedReport.Id);

            if (existingReport != null)
            {
                existingReport.HazardType = updatedReport.HazardType;
                existingReport.Description = updatedReport.Description;
                existingReport.Status = updatedReport.Status;
                existingReport.Photo = updatedReport.Photo;
            }

            return true;
        }

        public bool UpdateUser(User updatedUser)
        {
            var existingUser = _appDbContext.Users.SingleOrDefault(p => p.Id == updatedUser.Id);

            if (existingUser != null)
            {
                existingUser.UserName = updatedUser.UserName;
                existingUser.Email = updatedUser.Email;
                existingUser.PasswordHash = updatedUser.PasswordHash;
                existingUser.Photo = updatedUser.Photo;
                existingUser.PhoneNumber = updatedUser.PhoneNumber;
            }

            return true;
        }
    }
}
