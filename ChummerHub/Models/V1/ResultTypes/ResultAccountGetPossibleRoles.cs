using System;
using System.Collections.Generic;

namespace ChummerHub.Models.V1
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ResultAccountGetPossibleRoles'
    public class ResultAccountGetPossibleRoles : ResultBase
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ResultAccountGetPossibleRoles'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ResultAccountGetPossibleRoles.AllRoles'
        public List<string> AllRoles
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ResultAccountGetPossibleRoles.AllRoles'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ResultAccountGetPossibleRoles.ResultAccountGetPossibleRoles()'
        public ResultAccountGetPossibleRoles() => AllRoles = new List<string>();

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ResultAccountGetPossibleRoles.ResultAccountGetPossibleRoles(List<string>)'
        public ResultAccountGetPossibleRoles(List<string> list) => AllRoles = list;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ResultAccountGetPossibleRoles.ResultAccountGetPossibleRoles(Exception)'
        public ResultAccountGetPossibleRoles(Exception e) : base(e)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ResultAccountGetPossibleRoles.ResultAccountGetPossibleRoles(Exception)'
        {

        }
    }
}
