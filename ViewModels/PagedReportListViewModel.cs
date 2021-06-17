using X.PagedList;

namespace Nemesys.ViewModels
{
    public class PagedReportListViewModel
    {
        public IPagedList<ReportViewModel> Reports { get; set; }

        public PagedReportListViewModel(IPagedList<ReportViewModel> reports)
        {
            Reports = reports;
        }
    }
}
