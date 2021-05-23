using Nemesys.Models;
using System.Collections.Generic;

namespace Nemesys.ViewModels
{
    public class HallOfFameViewModel
    {
        public IEnumerable<User> HallOfFameUsers;

        public HallOfFameViewModel(IEnumerable<User> hallOfFameUsers)
        {
            HallOfFameUsers = hallOfFameUsers;
        }
    }
}