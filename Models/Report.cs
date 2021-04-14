using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public DateTime DateOfReport { get; set; }
        public DateTime DateTimeOfHazard { get; set; }
        public string HazardType { get; set; }
        public string Description { get; set; }
        public ReportStatus Status { get; set; }
        public User Account { get; set; }
        public int NumberOfStars { get; set; }
        public string Photo { get; set; }
    }
}
