using System.Collections.Generic;

namespace Nemesys.Models.Interfaces
{
    public interface INemesysRepository
    {
        IEnumerable<Report> GetStarredUserReports(string userId);
        IEnumerable<User> GetUsersWhoStarredReport(int reportId);
        bool StarReport(string userId, int reportId);

        IEnumerable<Report> GetAllReports();
        IEnumerable<Report> GetAllReportsWithStatus(int statusId);
        Report GetReportById(int reportId);
        Report CreateReport(Report report);
        bool UpdateReport(Report updatedReport);
        bool DeleteReport(int reportId);

        IEnumerable<User> GetUsers();
        IEnumerable<User> GetTopUsers(int amount);
        User GetUserById(string userId);

        Investigation CreateInvestigation(Investigation investigation, int statusId);
        Investigation GetInvestigationById(int investigationId);
        bool UpdateInvestigation(Investigation updatedInvestigation, int statusId);

        IEnumerable<HazardType> GetHazardTypes();
        HazardType GetHazardTypeById(int hazardId);

        IEnumerable<ReportStatus> GetReportStatuses();
        ReportStatus GetReportStatusById(int statusId);
    }
}
