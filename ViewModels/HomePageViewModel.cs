using System.Collections.Generic;

namespace Nemesys.ViewModels
{
    public class HomePageViewModel
    {
        public HallOfFameViewModel HallOfFame { get; set; }
        public PagedReportListViewModel Reports { get; set; }
        public IEnumerable<ReportStatusViewModel> Statuses { get; set; }

        public HomePageViewModel(HallOfFameViewModel hallOfFame, PagedReportListViewModel reports, IEnumerable<ReportStatusViewModel> statuses)
        {
            HallOfFame = hallOfFame;
            Reports = reports;
            Statuses = statuses;
        }
    }
}
