using System.Collections.Generic;

namespace Nemesys.ViewModels
{
    public class HomePageViewModel
    {
        public HallOfFameViewModel HallOfFame { get; set; }
        public ReportListViewModel Reports { get; set; }
        public IEnumerable<ReportStatusViewModel> Statuses { get; set; }

        public HomePageViewModel(HallOfFameViewModel hallOfFame, ReportListViewModel reports, IEnumerable<ReportStatusViewModel> statuses)
        {
            HallOfFame = hallOfFame;
            Reports = reports;
            Statuses = statuses;
        }
    }
}
