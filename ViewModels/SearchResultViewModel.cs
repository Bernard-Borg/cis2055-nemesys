using System.Collections.Generic;

namespace Nemesys.ViewModels
{
    public class SearchResultViewModel
    {
        public ReportListViewModel Reports { get; set; }

        public IEnumerable<ProfileCardViewModel> Users { get; set; }
        public string SearchString { get; set; }

        public SearchResultViewModel(ReportListViewModel reports, IEnumerable<ProfileCardViewModel> users, string searchString)
        {
            Reports = reports;
            Users = users;
            SearchString = searchString;
        }
    }
}
