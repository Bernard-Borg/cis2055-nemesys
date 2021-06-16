using Nemesys.Models;
using System;

namespace Nemesys.ViewModels
{
    public class InvestigationViewModel
    {
        public int InvestigationId { get; set; }
        public string Description { get; set; }
        public string DateOfAction { get; set; }
        public string ReportName { get; set; }
        public int ReportId { get; set; }
        public string ReportDescription { get; set; }
        public ReportStatusViewModel ReportStatus { get; set; }

        public ProfileCardViewModel Investigator { get; set; }

        public InvestigationViewModel(Investigation investigation) {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

            InvestigationId = investigation.InvestigationId;
            Description = investigation.Description;
            DateOfAction = TimeZoneInfo.ConvertTimeFromUtc(investigation.DateOfAction, timeZone).ToString("d MMMM yyyy 'at' HH:mm");
            ReportId = investigation.ReportId;
            ReportDescription = investigation.Report.Description;
            ReportStatus = new ReportStatusViewModel(investigation.Report.Status);
            
            Investigator = new ProfileCardViewModel(investigation.Investigator);  
        }
    }
}
