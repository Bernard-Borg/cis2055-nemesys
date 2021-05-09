using Nemesys.Models;
using System.Linq;

namespace Nemesys.ViewModels
{
    public class ReportViewModel
    {
        public User Author;
        public Report Report;
        public bool Starred;
        public string BootstrapStatus;
        public string StatusString;

        public ReportViewModel(Report report)
        {
            Report = report;
            Author = report.Author;
            //Get current user and check if user has starred report 
            //if(temp.Where(x => x.UserId == ))
            Starred = true;

            switch (Report.Status)
            {
                case ReportStatus.Open:
                    BootstrapStatus = "text-success";
                    StatusString = "Open";
                    break;
                case ReportStatus.UnderInvestigation:
                    BootstrapStatus = "text-warning";
                    StatusString = "Under Investigation";
                    break;
                case ReportStatus.NoActionRequired:
                    BootstrapStatus = "text-info";
                    StatusString = "No Action Required";
                    break;
                case ReportStatus.Closed:
                    BootstrapStatus = "text-danger";
                    StatusString = "Closed";
                    break;
            }
        }
    }
}
