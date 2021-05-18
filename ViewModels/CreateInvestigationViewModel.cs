using Nemesys.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.ViewModels
{
    public class CreateInvestigationViewModel
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
