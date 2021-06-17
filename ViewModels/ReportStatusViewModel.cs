using Nemesys.Models;

namespace Nemesys.ViewModels
{
    public class ReportStatusViewModel
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusColour { get; set; }

        public ReportStatusViewModel(ReportStatus status)
        {
            StatusId = status.Id;
            StatusName = status.StatusName;
            StatusColour = status.HexColour;
        }
    }
}
