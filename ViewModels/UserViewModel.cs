using Nemesys.Models;
using System;
using System.Collections.Generic;

namespace Nemesys.ViewModels
{
    public class UserViewModel
    {
        public ReportListViewModel Reports { get; set; }
        public int NumberOfStars { get; set; }
        public int NumberOfReports { get; set; }
        public string Email { get; set; }
        public string LastActiveDate { get; set; }
        public string DateJoined { get; set; }
        public string Photo { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
        public string UserBio { get; set; }
        public bool IsCurrentUser { get; set; }
        public List<RoleViewModel> Roles { get; set; }

        public UserViewModel(User user, User currentUser, IList<string> roles)
        {
            if (currentUser != null)
            {
                IsCurrentUser = user.Id.Equals(currentUser.Id);
            }
            else
            {
                IsCurrentUser = false;
            }

            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

            Reports = new ReportListViewModel(user.Reports, currentUser);
            NumberOfStars = user.NumberOfStars;
            NumberOfReports = user.NumberOfReports;
            Email = user.Email;
            LastActiveDate = TimeZoneInfo.ConvertTimeFromUtc(user.LastActiveDate, timeZone).ToString();
            DateJoined = TimeZoneInfo.ConvertTimeFromUtc(user.DateJoined, timeZone).ToShortDateString();
            Photo = user.Photo;
            Username = user.Alias;
            UserId = user.Id;
            UserBio = user.Bio;

            Roles = new List<RoleViewModel>();

            foreach (string role in roles)
            {
                Roles.Add(new RoleViewModel(role));
            }
        }
    }
}
