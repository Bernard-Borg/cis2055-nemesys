using Nemesys.Models;
using System.Collections.Generic;

namespace Nemesys.ViewModels
{
    public class HallOfFameViewModel
    {
        public List<User> HallOfFameUsers;

        public HallOfFameViewModel(List<User> hallOfFameUsers)
        {
            HallOfFameUsers = hallOfFameUsers;
        }
    }
}
