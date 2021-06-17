using Nemesys.Models;

namespace Nemesys.ViewModels
{
    public class HazardTypeViewModel
    {
        public int HazardId { get; set; }
        public string HazardName { get; set; }

        public HazardTypeViewModel(HazardType hazardType)
        {
            HazardId = hazardType.Id;
            HazardName = hazardType.HazardName;
        }
    }
}
