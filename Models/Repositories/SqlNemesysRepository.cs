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

        public bool ChangeDescription(int reportId, string description)
        {
            throw new NotImplementedException();
        }

        public bool ChangeInvestigationDateOfAction(int investigationId, DateTime dateOfAction)
        {
            throw new NotImplementedException();
        }

        public bool ChangeInvestigationDescription(int investigationId, string description)
        {
            throw new NotImplementedException();
        }

        public bool ChangeReportHazardDateTime(int reportId, DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public bool ChangeReportHazardType(int reportId, HazardType hazardType)
        {
            throw new NotImplementedException();
        }

        public bool ChangeReportStatus(int reportId, ReportStatus status)
        {
            throw new NotImplementedException();
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

        public bool EditEmail(int userId, string email)
        {
            throw new NotImplementedException();
        }

        public bool EditName(int userId, string name)
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
    }
}
