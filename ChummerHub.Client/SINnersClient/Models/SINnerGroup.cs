// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace SINners.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class SINnerGroup
    {
        /// <summary>
        /// Initializes a new instance of the SINnerGroup class.
        /// </summary>
        public SINnerGroup()
        {
        }

        /// <summary>
        /// Initializes a new instance of the SINnerGroup class.
        /// </summary>
        public SINnerGroup(Guid? id = default(Guid?), Guid? myParentGroupId = default(Guid?), bool? isPublic = default(bool?), string groupCreatorUserName = default(string), SINnerGroupSetting mySettings = default(SINnerGroupSetting), string groupname = default(string), string passwordHash = default(string), bool? hasPassword = default(bool?), string description = default(string), string language = default(string), IList<SINnerGroup> myGroups = default(IList<SINnerGroup>), string myAdminIdentityRole = default(string))
        {
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
            if (Groupname != null)
            {
                if (Groupname.Length > 64)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "Groupname", 64);
                }
            }
            if (Language != null)
            {
                if (Language.Length > 6)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "Language", 6);
                }
            }
            if (MyGroups != null)
            {
                foreach (SINnerGroup element in MyGroups)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
            if (MyAdminIdentityRole != null)
            {
                if (MyAdminIdentityRole.Length > 64)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "MyAdminIdentityRole", 64);
                }
            }
        }
    }
}
