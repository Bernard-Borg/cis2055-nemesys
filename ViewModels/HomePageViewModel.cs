namespace Nemesys.ViewModels
{
    public class HomePageViewModel
    {
        public HallOfFameViewModel HallOfFame;
        public ReportListViewModel Reports;

        public HomePageViewModel(HallOfFameViewModel hallOfFame, ReportListViewModel reports)
        {
            HallOfFame = hallOfFame;
            Reports = reports;
        }
    }
}
