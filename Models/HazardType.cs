using System.Collections.Generic;

namespace Nemesys.Models
{
    public class HazardType
    {
        public int Id { get; set; }
        public string HazardName { get; set; }

        public IEnumerable<Report> Reports { get; set; }
    }
}