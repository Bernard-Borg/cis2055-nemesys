using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models
{
    public class Investigation
    {
        public int InvestigationId { get; set; }
        public string Description { get; set; }
        public DateTime DateOfAction { get; set; }
        public User Investigator { get; set; }
        public Report Report { get; set; }
    }
}
