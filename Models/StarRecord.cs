using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models
{
    public class StarRecord
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int ReportId { get; set; }
        public Report Report { get; set; }
    }
}
