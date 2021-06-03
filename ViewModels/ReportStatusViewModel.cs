using Nemesys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nemesys.ViewModels
{
    public class ReportStatusViewModel
    {
        public string StatusName { get; set; }
        public string StatusColour { get; set; }

        public ReportStatusViewModel(ReportStatus status)
        {
            StatusName = status.StatusName;
            StatusColour = status.HexColour;
        }
    }
}
