// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace SINners.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class SINnerSearchGroup
    {
        /// <summary>
        /// Initializes a new instance of the SINnerSearchGroup class.
        /// </summary>
        public SINnerSearchGroup()
        {
        }

        /// <summary>
        /// Initializes a new instance of the SINnerSearchGroup class.
        /// </summary>
        public SINnerSearchGroup(IList<SINnerSearchGroup> mySINSearchGroups = default(IList<SINnerSearchGroup>), string errorText = default(string), IList<SINnerSearchGroupMember> myMembers = default(IList<SINnerSearchGroupMember>), Guid? id = default(Guid?), Guid? myParentGroupId = default(Guid?), bool? isPublic = default(bool?), string groupCreatorUserName = default(string), SINnerGroupSetting mySettings = default(SINnerGroupSetting), string groupname = default(string), string passwordHash = default(string), bool? hasPassword = default(bool?), string description = default(string), string language = default(string), IList<SINnerGroup> myGroups = default(IList<SINnerGroup>), string myAdminIdentityRole = default(string))
        {
            MySINSearchGroups = mySINSearchGroups;
            ErrorText = errorText;
            MyMembers = myMembers;
            Id = id;
            MyParentGroupId = myParentGroupId;
            IsPublic = isPublic;
            GroupCreatorUserName = groupCreatorUserName;
            MySettings = mySettings;
            Groupname = groupname;
            PasswordHash = passwordHash;
            HasPassword = hasPassword;
            Description = description;
            Language = language;
            MyGroups = myGroups;
            MyAdminIdentityRole = myAdminIdentityRole;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "mySINSearchGroups")]
        public IList<SINnerSearchGroup> MySINSearchGroups
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "errorText")]
        public string ErrorText
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "myMembers")]
        public IList<SINnerSearchGroupMember> MyMembers
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public Guid? Id
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "myParentGroupId")]
        public Guid? MyParentGroupId
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "isPublic")]
        public bool? IsPublic
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "groupCreatorUserName")]
        public string GroupCreatorUserName
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "mySettings")]
        public SINnerGroupSetting MySettings
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "groupname")]
        public string Groupname
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "passwordHash")]
        public string PasswordHash
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "hasPassword")]
        public bool? HasPassword
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "language")]
        public string Language
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "myGroups")]
        public IList<SINnerGroup> MyGroups
        {
            get; set;
        }

        /// <summary>
        /// Only users of the specified Role can join this group
        /// </summary>
        [JsonProperty(PropertyName = "myAdminIdentityRole")]
        public string MyAdminIdentityRole
        {
            get; set;
        }

        /// <summary>
        /// Validate the object. Throws ValidationException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            if (this.MySINSearchGroups != null)
            {
                foreach (SINnerSearchGroup element in this.MySINSearchGroups)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
            if (this.MyMembers != null)
            {
                foreach (SINnerSearchGroupMember element1 in this.MyMembers)
                {
                    if (element1 != null)
                    {
                        element1.Validate();
                    }
                }
            }
            if (this.Groupname != null)
            {
                if (this.Groupname.Length > 64)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "Groupname", 64);
                }
            }
            if (this.Language != null)
            {
                if (this.Language.Length > 6)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "Language", 6);
                }
            }
            if (this.MyGroups != null)
            {
                foreach (SINnerGroup element2 in this.MyGroups)
                {
                    if (element2 != null)
                    {
                        element2.Validate();
                    }
                }
            }
            if (this.MyAdminIdentityRole != null)
            {
                if (this.MyAdminIdentityRole.Length > 64)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "MyAdminIdentityRole", 64);
                }
            }
        }
    }
}
