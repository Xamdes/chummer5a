using System.Collections.Generic;

namespace SINners.Models
{
    public partial class SINnerSearchGroup
    {

        public SINnerSearchGroup(SINnerGroup myGroup)
        {
            MyMembers = new List<SINnerSearchGroupMember>();

            Id = myGroup.Id;
            MyParentGroupId = myGroup.MyParentGroupId;
            IsPublic = myGroup.IsPublic;
            MySettings = myGroup.MySettings;
            Groupname = myGroup.Groupname;
            //PasswordHash = myGroup.PasswordHash;
            Language = myGroup.Language;
            MyGroups = myGroup.MyGroups;
            MyAdminIdentityRole = myGroup.MyAdminIdentityRole;
        }

        public override string ToString() => GroupDisplayname;

        public string GroupDisplayname
        {
            get
            {
                string ret = Groupname;
                if (!(string.IsNullOrEmpty(Language)))
                {
                    //if ((this.MyMembers != null)
                    //    && (this.MyMembers.Any()))
                    //{
                    //    ret += ": " + MyMembers.Count + " members";
                    //}
                }
                return ret;
            }
        }
    }
}
