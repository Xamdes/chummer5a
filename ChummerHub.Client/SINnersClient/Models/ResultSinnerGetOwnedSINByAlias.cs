﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace SINners.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public partial class ResultSinnerGetOwnedSINByAlias
    {
        /// <summary>
        /// Initializes a new instance of the ResultSinnerGetOwnedSINByAlias
        /// class.
        /// </summary>
        public ResultSinnerGetOwnedSINByAlias()
        {
        }

        /// <summary>
        /// Initializes a new instance of the ResultSinnerGetOwnedSINByAlias
        /// class.
        /// </summary>
        public ResultSinnerGetOwnedSINByAlias(IList<SINner> mySINners = default(IList<SINner>), object myException = default(object), bool? callSuccess = default(bool?), string errorText = default(string))
        {
            MySINners = mySINners;
            MyException = myException;
            CallSuccess = callSuccess;
            ErrorText = errorText;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "mySINners")]
        public IList<SINner> MySINners
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

    }
}
