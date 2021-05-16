using Nemesys.Models;
using System.Collections.Generic;

namespace Nemesys.ViewModels
{
    public class HomePageViewModel
    {
        public HallOfFameViewModel HallOfFame;
        public ReportListViewModel Reports;
        public IEnumerable<ReportStatus> Statuses;

        public HomePageViewModel(HallOfFameViewModel hallOfFame, ReportListViewModel reports, IEnumerable<ReportStatus> statuses)
        {
            HallOfFame = hallOfFame;
            Reports = reports;
            Statuses = statuses;
        }
    }
}
