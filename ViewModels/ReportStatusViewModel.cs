using Nemesys.Models;

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
