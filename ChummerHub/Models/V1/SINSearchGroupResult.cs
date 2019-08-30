using System.Collections.Generic;

namespace ChummerHub.Models.V1
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINSearchGroupResult'
    public class SINSearchGroupResult
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINSearchGroupResult'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINSearchGroupResult.Roles'
        public List<string> Roles
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINSearchGroupResult.Roles'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINSearchGroupResult.Version'
        public ChummerHubVersion Version
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINSearchGroupResult.Version'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINSearchGroupResult.SINSearchGroupResult()'
        public SINSearchGroupResult()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINSearchGroupResult.SINSearchGroupResult()'
        {
            SINGroups = new List<SINnerSearchGroup>();
            Roles = new List<string>();
            Version = new ChummerHubVersion();
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINSearchGroupResult.SINGroups'
        public List<SINnerSearchGroup> SINGroups
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINSearchGroupResult.SINGroups'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINSearchGroupResult.ErrorText'
        public string ErrorText
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINSearchGroupResult.ErrorText'
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroup'
    public class SINnerSearchGroup : SINnerGroup
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroup'
    {

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroup.MySINSearchGroups'
        public List<SINnerSearchGroup> MySINSearchGroups
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroup.MySINSearchGroups'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroup.ErrorText'
        public string ErrorText
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroup.ErrorText'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroup.MyMembers'
        public List<SINnerSearchGroupMember> MyMembers
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroup.MyMembers'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroup.SINnerSearchGroup()'
        public SINnerSearchGroup()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroup.SINnerSearchGroup()'
        {
            MyMembers = new List<SINnerSearchGroupMember>();
            MyGroups = new List<SINnerGroup>();
            MySINSearchGroups = new List<SINnerSearchGroup>();
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroup.SINnerSearchGroup(SINnerGroup)'
        public SINnerSearchGroup(SINnerGroup groupbyname)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroup.SINnerSearchGroup(SINnerGroup)'
        {
            MyParentGroupId = groupbyname?.MyParentGroupId;
            Id = groupbyname?.Id;
            if (groupbyname != null)
            {
                IsPublic = groupbyname.IsPublic;
            }

            Groupname = groupbyname?.Groupname;
            MyMembers = new List<SINnerSearchGroupMember>();
            MyGroups = new List<SINnerGroup>();
            MySINSearchGroups = new List<SINnerSearchGroup>();
            MyAdminIdentityRole = groupbyname?.MyAdminIdentityRole;
            Language = groupbyname?.Language;
            PasswordHash = groupbyname?.PasswordHash;
            MySettings = groupbyname?.MySettings;
        }

    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroupMember'
    public class SINnerSearchGroupMember
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroupMember'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroupMember.MySINner'
        public SINner MySINner
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroupMember.MySINner'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroupMember.Username'
        public string Username
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroupMember.Username'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINnerSearchGroupMember.SINnerSearchGroupMember()'
        public SINnerSearchGroupMember() => MySINner = new SINner();

    }

}

