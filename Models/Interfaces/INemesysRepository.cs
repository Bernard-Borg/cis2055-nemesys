using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models.Interfaces
{
    public interface INemesysRepository
    {
        List<Report> GetStarredUserReports(int userId);
        List<User> GetUsersWhoStarredReport(int reportId);
        bool StarReport(int userId, int reportId);

        List<Report> GetAllReports();
        List<Report> GetAllReportsWithStatus(ReportStatus status);
        Report GetReportById(int reportId);
        Report CreateReport(Report report);
        bool ChangeReportStatus(int reportId, ReportStatus status);
        bool ChangeReportHazardDateTime(int reportId, DateTime dateTime);
        bool ChangeReportHazardType(int reportId, HazardType hazardType);
        bool ChangeDescription(int reportId, string description);
        bool DeleteReport(int reportId);

        List<User> GetUsers();
        List<User> GetTopUsers(int amount);
        bool DeleteUser(int userId);
        User GetUserById(int userId);
        bool PromoteUser(int userId);
        bool EditName(int userId, string name);
        bool EditEmail(int userId, string email);

        Investigation CreateInvestigation(int reportId);
        List<Investigation> GetAllInvestigations();
        Investigation GetInvestigationById(int investigationId);
        bool ChangeInvestigationDescription(int investigationId, string description);
        bool ChangeInvestigationDateOfAction(int investigationId, DateTime dateOfAction);
        bool RemoveInvestigation(int investigationId);
    }
}
