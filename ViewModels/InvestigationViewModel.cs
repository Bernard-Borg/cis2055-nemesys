using System;
using System.Collections.Generic;
using System.Linq;
using Nemesys.Models;

namespace Nemesys.ViewModels
{
    public class InvestigationViewModel
    {
        public string Description { get; set; }
        public string DateOfAction { get; set; }
        public string ReportName { get; set; }
        public string InvestigatorId { get; set; }
        public int ReportId { get; set; }
        public string ReportDescription { get; set; }
        public string StatusName { get; set; }
        public string StatusColour { get; set; }

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
