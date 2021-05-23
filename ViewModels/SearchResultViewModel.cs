using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.ViewModels
{
    public class SearchResultViewModel
    {
        public ReportListViewModel Reports;
        public string SearchString;

        public SearchResultViewModel(ReportListViewModel reports, string searchString)
        {
            Reports = reports;
            SearchString = searchString;
        }
    }
}
