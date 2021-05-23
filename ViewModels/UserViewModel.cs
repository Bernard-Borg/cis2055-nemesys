using Nemesys.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nemesys.ViewModels
{
    public class UserViewModel
    {
        public ReportListViewModel Reports;
        public int NumberOfStars;
        public int NumberOfReports;
        public string LastActiveDate;
        public string DateJoined;
        public string Photo;
        public string Username;
        public string UserId;
        public string UserBio;
        public bool IsCurrentUser;
        public List<RoleViewModel> Roles;

        public UserViewModel(User user, User currentUser, IList<string> roles)
        {
            if (currentUser != null)
            {
                IsCurrentUser = user.Id.Equals(currentUser.Id);
            } else
            {
                IsCurrentUser = false;
            }

            Reports = new ReportListViewModel(user.Reports, currentUser);
            NumberOfStars = user.NumberOfStars;
            NumberOfReports = user.NumberOfReports;
            LastActiveDate = user.LastActiveDate.ToShortDateString();
            DateJoined = user.DateJoined.ToShortDateString();
            Photo = user.Photo;
            Username = user.Alias;
            UserId = user.Id;
            UserBio = user.Bio;

            Roles = new List<RoleViewModel>();

            foreach(string role in roles)
            {
                Roles.Add(new RoleViewModel(role));
            }
        }
    }
}
