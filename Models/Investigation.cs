using System;

namespace Nemesys.Models
{
    public class Investigation
    {
        public int InvestigationId { get; set; }
        public string Description { get; set; }
        public DateTime DateOfAction { get; set; }

        public string UserId { get; set; }
        public User Investigator { get; set; }

        public int ReportId { get; set; }
        public Report Report { get; set; }
    }
}
