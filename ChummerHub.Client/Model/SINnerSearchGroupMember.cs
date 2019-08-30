namespace SINners.Models
{
    public partial class SINnerSearchGroupMember
    {
        public string Display
        {
            get
            {
                string display = this.MySINner.Alias;
                if (!string.IsNullOrEmpty(Username))
                    display += " " + this.Username;
                return display;
            }
        }
    }
}
