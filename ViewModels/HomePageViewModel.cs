using Nemesys.Models;
using System.Collections.Generic;

namespace Nemesys.ViewModels
{
    public class HomePageViewModel
    {
        public HallOfFameViewModel HallOfFame { get; set; }
        public ReportListViewModel Reports { get; set; }
        public IEnumerable<ReportStatus> Statuses { get; set; }

        public HomePageViewModel(HallOfFameViewModel hallOfFame, ReportListViewModel reports, IEnumerable<ReportStatus> statuses)
        {
            HallOfFame = hallOfFame;
            Reports = reports;
            Statuses = statuses;
        }
    }
}
