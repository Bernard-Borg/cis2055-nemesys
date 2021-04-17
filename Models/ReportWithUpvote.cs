using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models
{
    public class ReportWithUpvote
    {
        public Report Report { get; set; }
        public bool IsUpvoted { get; set; }

        public ReportWithUpvote(Report report, bool upvoted)
        {
            Report = report;
            IsUpvoted = upvoted;
        }
    }
}
