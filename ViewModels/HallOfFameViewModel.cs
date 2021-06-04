using Nemesys.Models;
using System.Collections.Generic;

namespace Nemesys.ViewModels
{
    public class HallOfFameViewModel
    {
        public IEnumerable<User> HallOfFameUsers { get; set; }

        public HallOfFameViewModel(IEnumerable<User> hallOfFameUsers)
        {
            HallOfFameUsers = hallOfFameUsers;
        }
    }
}