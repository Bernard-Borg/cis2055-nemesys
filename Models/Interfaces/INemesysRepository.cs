using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        bool DeleteUser(string userId);
        User GetUserById(string userId);
        bool UpdateUser(User updatedUser);
        
        Investigation CreateInvestigation(Investigation investigation);
        IEnumerable<Investigation> GetAllInvestigations();
        Investigation GetInvestigationById(int investigationId);
        bool UpdateInvestigation(Investigation updatedInvestigation);
        bool RemoveInvestigation(int investigationId);

        IEnumerable<HazardType> GetHazardTypes();
        HazardType GetHazardTypeById(int hazardId);

        IEnumerable<ReportStatus> GetReportStatuses();
        ReportStatus GetReportStatusById(int statusId);
    }
}
