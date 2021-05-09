using System;
using System.Collections.Generic;

namespace Nemesys.Models
{
    public class Report
    {
        public int Id { get; set; }
        public DateTime DateOfReport { get; set; }
        public DateTime DateTimeOfHazard { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
        public HazardType HazardType { get; set; }
        public string Description { get; set; }
        public ReportStatus Status { get; set; }
        public string Photo { get; set; }

        public int NumberOfStars { get; set; }

        public string UserId { get; set; }
        public User Author { get; set; }

        public List<StarRecord> UsersWhichHaveStarred { get; set; }
    }
}
