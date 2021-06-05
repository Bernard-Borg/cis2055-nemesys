using Nemesys.Models;

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

        public string InvestigatorId { get; set; }
        public string InvestigatorUserName { get; set; }
        public string InvestigatorEmail { get; set; }
        public string InvestigatorPhoto { get; set; }
        public int InvestigatorStarsNumber { get; set; }

        public InvestigationViewModel(Investigation investigation) {
            Description = investigation.Description;
            DateOfAction = investigation.DateOfAction.ToString();
            ReportId = investigation.ReportId;
            ReportDescription = investigation.Report.Description;
            ReportStatus = new ReportStatusViewModel(investigation.Report.Status);

            InvestigationId = investigation.InvestigationId;
            InvestigatorId = investigation.Investigator.Id;
            InvestigatorUserName = investigation.Investigator.Alias;
            InvestigatorEmail = investigation.Investigator.Email;
            InvestigatorPhoto = investigation.Investigator.Photo;
            InvestigatorStarsNumber = investigation.Investigator.NumberOfStars;     
        }
    }
}
