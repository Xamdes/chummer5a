// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace SINners.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public partial class Tag
    {
        /// <summary>
        /// Initializes a new instance of the Tag class.
        /// </summary>
        public Tag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Tag class.
        /// </summary>
        public Tag(Guid? id = default(Guid?), string tagName = default(string), string tagValue = default(string), double? tagValueDouble = default(double?), string tagComment = default(string), Guid? parentTagId = default(Guid?), Guid? siNnerId = default(Guid?), IList<Tag> tags = default(IList<Tag>), bool? isUserGenerated = default(bool?), string tagType = default(string))
        {
            Id = id;
            TagName = tagName;
            TagValue = tagValue;
            TagValueDouble = tagValueDouble;
            TagComment = tagComment;
            ParentTagId = parentTagId;
            SiNnerId = siNnerId;
            Tags = tags;
            IsUserGenerated = isUserGenerated;
            TagType = tagType;
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
        [JsonProperty(PropertyName = "tagName")]
        public string TagName
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "tagValue")]
        public string TagValue
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "tagValueDouble")]
        public double? TagValueDouble
        {
            get; set;
        }

        /// <summary>
        /// This has NO FUNCTION and is only here for Debugging reasons.
        /// </summary>
        [JsonProperty(PropertyName = "tagComment")]
        public string TagComment
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "parentTagId")]
        public Guid? ParentTagId
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "siNnerId")]
        public Guid? SiNnerId
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "tags")]
        public IList<Tag> Tags
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "isUserGenerated")]
        public bool? IsUserGenerated
        {
            get; set;
        }

        /// <summary>
        /// Possible values include: 'list', 'bool', 'int', 'Guid', 'string',
        /// 'double', 'binary', 'enum', 'other', 'unknown'
        /// </summary>
        [JsonProperty(PropertyName = "tagType")]
        public string TagType
        {
            get; set;
        }

        /// <summary>
        /// Validate the object. Throws ValidationException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            if (this.TagName != null)
            {
                if (this.TagName.Length > 64)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "TagName", 64);
                }
            }
            if (this.TagValue != null)
            {
                if (this.TagValue.Length > 64)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "TagValue", 64);
                }
            }
            if (this.TagComment != null)
            {
                if (this.TagComment.Length > 64)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "TagComment", 64);
                }
            }
            if (this.Tags != null)
            {
                foreach (Tag element in this.Tags)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
        }
    }
}
