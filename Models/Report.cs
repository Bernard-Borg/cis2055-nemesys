using System.Threading.Tasks;

namespace cis2205-nemesys.Models
{
    public class Report
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Title { get; set; }
        public string TypeOfHazard { get; set; }
        public string Imageurl { get; set; }
        public string Description { get; set; }
        public DateTime InvestigationDate { get; set; }
        public string InvestigationStatus { get; set; }
    }
}