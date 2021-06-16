using Nemesys.CustomAttributes;
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
        [UIHint("TextareaWithCounter")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Date of action is required")]
        [Display(Name = "Date of action")]
        [DateRange]
        [UIHint("Date")]
        public DateTime? DateOfAction { get; set; }
        
        [Required(ErrorMessage = "An investigation status is required")]
        [Display(Name = "Status of investigation")]
        public int? StatusId { get; set; }

        //Used to fill up drop down
        public List<ReportStatusViewModel> ReportStatuses { get; set; }
    }
}