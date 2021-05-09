using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models.Interfaces
{
    public interface INemesysRepository
    {
        List<Report> GetStarredUserReports(string userId);
        List<User> GetUsersWhoStarredReport(int reportId);
        bool StarReport(string userId, int reportId);

        List<Report> GetAllReports();
        List<Report> GetAllReportsWithStatus(ReportStatus status);
        Report GetReportById(int reportId);
        Report CreateReport(Report report);
        bool UpdateReport(Report updatedReport);
        bool DeleteReport(int reportId);

        List<User> GetUsers();
        List<User> GetTopUsers(int amount);
        bool DeleteUser(string userId);
        User GetUserById(string userId);
        bool UpdateUser(User updatedUser);

        Investigation CreateInvestigation(Investigation investigation);
        List<Investigation> GetAllInvestigations();
        Investigation GetInvestigationById(int investigationId);
        bool UpdateInvestigation(Investigation updatedInvestigation);
        bool RemoveInvestigation(int investigationId);
    }
}
