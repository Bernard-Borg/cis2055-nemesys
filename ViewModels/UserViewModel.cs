using Nemesys.Models;
using System.Linq;

namespace Nemesys.ViewModels
{
    public class UserViewModel
    {
        public User User;

        public UserViewModel(User user)
        {
            User = user;
        }
    }
}
