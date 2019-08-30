using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace SINners.Models
{
    [System.Diagnostics.DebuggerDisplay("{Display}")]
    public partial class Tag
    {
        [IgnoreDataMember]
        [XmlIgnore]
        [JsonIgnore]
        public Tag MyParentTag
        {
            get; set;
        }

        public Tag(object myRuntimeObject, Chummer.HubTagAttribute hubTag)
        {
            this.Id = Guid.NewGuid();
            MyRuntimeObject = myRuntimeObject;
            MyRuntimeHubTag = hubTag;
            this.Tags = new List<Tag>();
        }

        public Tag(bool isUserGenerated)
        {
            this.Id = Guid.NewGuid();
            this.IsUserGenerated = isUserGenerated;
            this.Tags = new List<Tag>();
        }

        public Tag(object myRuntimeObject, Chummer.HubClassTagAttribute hubClassTag)
        {
            this.Id = Guid.NewGuid();
            MyRuntimeObject = myRuntimeObject;
            MyRuntimeHubClassTag = hubClassTag;
            this.Tags = new List<Tag>();
        }

        [IgnoreDataMember]
        [XmlIgnore]
        [JsonIgnore]
        public string Display
        {
            get
            {
                string str = "";
                Tag tempParent = this;
                while (tempParent != null)
                {
                    string tempstr = tempParent.TagName;
                    if (!string.IsNullOrEmpty(tempParent.TagValue))
                        tempstr += ": " + tempParent.TagValue;
                    if (!string.IsNullOrEmpty(str))
                        tempstr += " -> " + str;
                    str = tempstr;
                    tempParent = tempParent.MyParentTag;
                }
                if (!string.IsNullOrEmpty(this.TagComment))
                    str += " (" + this.TagComment + ")";
                return str;
            }

        }

        [IgnoreDataMember]
        [XmlIgnore]
        [JsonIgnore]
        public bool DeleteIfEmpty
        {
            get; set;
        }

        [IgnoreDataMember]
        [XmlIgnore]
        [JsonIgnore]
        public object MyRuntimeObject
        {
            get; set;
        }

        [IgnoreDataMember]
        [XmlIgnore]
        [JsonIgnore]
        public Chummer.HubTagAttribute MyRuntimeHubTag
        {
            get; set;
        }

        [IgnoreDataMember]
        [XmlIgnore]
        [JsonIgnore]
        public Chummer.HubClassTagAttribute MyRuntimeHubClassTag
        {
            get; set;
        }

        internal void SetSinnerIdRecursive(Guid? id)
        {
            this.SiNnerId = id;
            foreach (Tag childtag in this.Tags)
                childtag.SetSinnerIdRecursive(id);
        }

    }
}
