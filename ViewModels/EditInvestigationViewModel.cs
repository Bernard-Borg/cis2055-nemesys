using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nemesys.ViewModels
{
    public class EditInvestigationViewModel
    {
        public int InvestigationId { get; set; }

        [Required(ErrorMessage = "Investigation description is required")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters long")]
        [MaxLength(255, ErrorMessage = "Description cannot be longer than 255 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Date of action is required")]
        [Display(Name = "Date of action")]
        public DateTime? DateOfAction { get; set; }

        public string UserId { get; set; }
        public int ReportId { get; set; }

        //Used to fill up drop down
        public List<ReportStatusViewModel> ReportStatuses { get; set; }
    }
}
