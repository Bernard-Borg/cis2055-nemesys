using Nemesys.Models;

namespace Nemesys.ViewModels
{
    public class ProfileCardViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public int StarsCount { get; set; }
        public int ReportCount { get; set; }

        public ProfileCardViewModel(User user)
        {
            Id = user.Id;
            Username = user.Alias;
            Email = user.Email;
            Photo = user.Photo;
            StarsCount = user.NumberOfStars;
            ReportCount = user.NumberOfReports;
        }
    }
}
