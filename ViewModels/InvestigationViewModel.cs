using System;
using System.Collections.Generic;
using System.Linq;
using Nemesys.Models;

namespace Nemesys.ViewModels
{
    public class InvestigationViewModel
    {
        public int InvestigationId;
        public string Description;
        public string DateOfAction;
        public string ReportName;
        public int ReportId;
        public string ReportDescription;
        public string StatusName;
        public string StatusColour;

        public string InvestigatorId;
        public string InvestigatorUserName;
        public string InvestigatorEmail;
        public string InvestigatorPhoto;
        public int InvestigatorStarsNumber;

        public InvestigationViewModel(Investigation investigation) {
            Description = investigation.Description;
            DateOfAction = investigation.DateOfAction.ToShortDateString();
            ReportId = investigation.ReportId;
            ReportDescription = investigation.Report.Description;
            StatusName = investigation.Report.Status.StatusName;
            StatusColour = investigation.Report.Status.HexColour;

            InvestigatorId = investigation.Investigator.Id;
            InvestigatorUserName = investigation.Investigator.Alias;
            InvestigatorEmail = investigation.Investigator.Email;
            InvestigatorPhoto = investigation.Investigator.Photo;
            InvestigatorStarsNumber = investigation.Investigator.NumberOfStars;     
        }
    }
}
