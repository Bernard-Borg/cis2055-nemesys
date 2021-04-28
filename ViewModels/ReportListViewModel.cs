using Nemesys.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nemesys.ViewModels
{
    public class ReportListViewModel
    {
        public List<ReportViewModel> ReportViewModels;

        public ReportListViewModel(List<Report> reports)
        {
            ReportViewModels = reports.Select(report => new ReportViewModel(report)).ToList();
        }
    }
}