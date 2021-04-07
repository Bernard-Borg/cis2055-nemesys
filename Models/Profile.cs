using System.Threading.Tasks;

namespace cis2205-nemesys.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        /*To check*/
        public string Password{ get; set; }
        public string Imageurl { get; set; }
        public string Usertype { get; set; }
    }
}