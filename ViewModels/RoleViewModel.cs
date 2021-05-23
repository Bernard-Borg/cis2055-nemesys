namespace Nemesys.ViewModels
{
    public class RoleViewModel
    {
        public string RoleName;
        public string BackgroundColour;
        public string TextColour;

        public RoleViewModel(string role)
        {
            RoleName = role;

            if (role == "Admin")
            {
                BackgroundColour = "#DC3545";
                TextColour = "#FFFFFF";
            } else if (role == "Investigator")
            {
                BackgroundColour = "#98643C";
                TextColour = "#FFFFFF";
            } else if (role == "Reporter")
            {
                BackgroundColour = "#28A745";
                TextColour = "#FFFFFF";
            }
        }
    }
}
