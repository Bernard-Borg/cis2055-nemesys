using System;
using System.Collections.Generic;
using System.Linq;
using Nemesys.Models;

namespace Nemesys.ViewModels
{
    public class InvestigationViewModel
    {
        public string Description;
        public string DateOfAction;
        public string ReportName;
        public string InvestigatorId;
        public int ReportId;
        public string ReportDescription;
        public string StatusName;
        public string StatusColour;

        public InvestigationViewModel(Investigation investigation) {
            Description = investigation.Description;
            DateOfAction = investigation.DateOfAction.ToShortDateString();
            InvestigatorId = investigation.UserId;
            ReportId = investigation.ReportId;
            ReportDescription = investigation.Report.Description;
            StatusName = investigation.Report.Status.StatusName;
            StatusColour = investigation.Report.Status.HexColour;
        }
    }
}
