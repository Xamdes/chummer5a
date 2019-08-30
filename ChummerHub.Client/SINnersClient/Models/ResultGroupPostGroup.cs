﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace SINners.Models
{
    using Newtonsoft.Json;

    public partial class ResultGroupPostGroup
    {
        /// <summary>
        /// Initializes a new instance of the ResultGroupPostGroup class.
        /// </summary>
        public ResultGroupPostGroup()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ResultGroupPostGroup class.
        /// </summary>
        public ResultGroupPostGroup(SINnerGroup myGroup = default(SINnerGroup), object myException = default(object), bool? callSuccess = default(bool?), string errorText = default(string))
        {
            MyGroup = myGroup;
            MyException = myException;
            CallSuccess = callSuccess;
            ErrorText = errorText;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "myGroup")]
        public SINnerGroup MyGroup
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "myException")]
        public object MyException
        {
            get; set;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "callSuccess")]
        public bool? CallSuccess
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
        /// Validate the object. Throws ValidationException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            if (MyGroup != null)
            {
                MyGroup.Validate();
            }
        }
    }
}
