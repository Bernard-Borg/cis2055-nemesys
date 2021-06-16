using Nemesys.Models;
using System.Collections.Generic;

namespace Nemesys.ViewModels
{
    public class HallOfFameViewModel
    {
        public IEnumerable<ProfileCardViewModel> HallOfFameUsers { get; set; }

        public HallOfFameViewModel(IEnumerable<ProfileCardViewModel> hallOfFameUsers)
        {
            HallOfFameUsers = hallOfFameUsers;
        }
    }
}