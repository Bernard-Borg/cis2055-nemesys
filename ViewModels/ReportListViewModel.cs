using Nemesys.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nemesys.ViewModels
{
    public class ReportListViewModel
    {
        public List<ReportViewModel> ReportViewModels { get; set; }

        public ReportListViewModel(List<Report> reports, User currentUser)
        {
            if (reports != null) {
                ReportViewModels = reports
                    .Select(report => new ReportViewModel(report, currentUser))
                    .ToList();
            } else
            {
                ReportViewModels = new List<ReportViewModel>();
            }
        }
    }
}