using System;
using System.ComponentModel.DataAnnotations;

namespace Nemesys.ViewModels
{
    public class EditInvestigationViewModel
    {
        public int InvestigationId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime DateOfAction { get; set; }

        public string UserId { get; set; }
        public int ReportId { get; set; }
    }
}
