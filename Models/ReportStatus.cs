using System;
using System.Collections.Generic;

namespace Nemesys.Models
{
    public class ReportStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        public string HexColour { get; set; }

        public IEnumerable<Report> Reports { get; set; }
    }
}