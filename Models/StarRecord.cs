using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models
{
    public class StarRecord
    {
        public int UserId { get; set; }
        public int ReportId { get; set; }
        public bool Starred { get; set; }
    }
}
