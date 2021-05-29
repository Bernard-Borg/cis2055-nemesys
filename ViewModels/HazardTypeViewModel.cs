using Nemesys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
