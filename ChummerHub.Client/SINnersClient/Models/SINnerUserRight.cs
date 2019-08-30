﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace SINners.Models
{
    using Newtonsoft.Json;
    using System;

    public partial class SINnerUserRight
    {
        /// <summary>
        /// Initializes a new instance of the SINnerUserRight class.
        /// </summary>
        public SINnerUserRight()
        {
        }

        /// <summary>
        /// Initializes a new instance of the SINnerUserRight class.
        /// </summary>
        public SINnerUserRight(Guid? id = default(Guid?), string eMail = default(string), bool? canEdit = default(bool?))
        {
            Id = id;
            EMail = eMail;
            CanEdit = canEdit;
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
        [JsonProperty(PropertyName = "eMail")]
        public string EMail
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "canEdit")]
        public bool? CanEdit
        {
            get; set;
        }

        /// <summary>
        /// Validate the object. Throws ValidationException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            if (this.EMail != null)
            {
                if (this.EMail.Length > 64)
                {
                    throw new ValidationException(ValidationRules.MaxLength, "EMail", 64);
                }
            }
        }
    }
}
