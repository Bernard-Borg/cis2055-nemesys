namespace Nemesys.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int NumberOfReports { get; set; }
        public int NumberOfUpvotes { get; set; }
        public string Photo { get; set; }
        public string Phone { get; set; }
        public UserType TypeOfUser { get; set; }
    }
}