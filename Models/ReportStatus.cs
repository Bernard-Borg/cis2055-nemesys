using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.Models
{
    public enum ReportStatus
    {
        Open = 0, 
        ActionRequired = 1, 
        UnderInvestigation = 2, 
        Closed = 3
    }
}
