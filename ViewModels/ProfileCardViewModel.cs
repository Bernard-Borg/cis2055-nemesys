using Nemesys.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nemesys.ViewModels
{
    public class ProfileCardViewModel
    {
        public string Role { get; set; }
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public int StarsCount { get; set; }
        public int ReportCount { get; set; }

        public ProfileCardViewModel(User user, string role = "N/A")
        {
            Role = role;
            Id = user.Id;
            Username = user.Alias;
            Email = user.Email;
            Photo = user.Photo;
            StarsCount = user.NumberOfStars;
            ReportCount = user.NumberOfReports;
        }
    }
}
