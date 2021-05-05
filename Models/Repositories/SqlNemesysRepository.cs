using Nemesys.Data;
using Nemesys.Models.Interfaces;
using System;
using System.Collections.Generic;

namespace Nemesys.Models.Repositories
{
    public class SqlNemesysRepository : INemesysRepository
    {
        private readonly AppDbContext _appDbContext;

        public SqlNemesysRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Investigation CreateInvestigation(int reportId)
        {
            throw new NotImplementedException();
        }

        public Report CreateReport(Report report)
        {
            throw new NotImplementedException();
        }

        public bool DeleteReport(int reportId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public List<Investigation> GetAllInvestigations()
        {
            throw new NotImplementedException();
        }

        public List<Report> GetAllReports()
        {
            throw new NotImplementedException();
        }

        public List<Report> GetAllReportsWithStatus(ReportStatus status)
        {
            throw new NotImplementedException();
        }

        public Investigation GetInvestigationById(int investigationId)
        {
            throw new NotImplementedException();
        }

        public Report GetReportById(int reportId)
        {
            throw new NotImplementedException();
        }

        public List<Report> GetStarredUserReports(int userId)
        {
            throw new NotImplementedException();
        }

        public List<User> GetTopUsers(int amount)
        {
            throw new NotImplementedException();
        }

        public User GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public List<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public List<User> GetUsersWhoStarredReport(int reportId)
        {
            throw new NotImplementedException();
        }

        public bool PromoteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public bool RemoveInvestigation(int investigationId)
        {
            throw new NotImplementedException();
        }

        public bool StarReport(int userId, int reportId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateInvestigation(Investigation updatedInvestigation)
        {
            throw new NotImplementedException();
        }

        public bool UpdateReport(Report updatedReport)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(User updatedUser)
        {
            throw new NotImplementedException();
        }
    }
}
