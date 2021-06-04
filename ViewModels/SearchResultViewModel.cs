namespace Nemesys.ViewModels
{
    public class SearchResultViewModel
    {
        public ReportListViewModel Reports { get; set; }
        public string SearchString { get; set; }

        public SearchResultViewModel(ReportListViewModel reports, string searchString)
        {
            Reports = reports;
            SearchString = searchString;
        }
    }
}
