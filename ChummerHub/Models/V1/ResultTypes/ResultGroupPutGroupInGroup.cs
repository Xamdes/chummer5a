using System;

namespace ChummerHub.Models.V1
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ResultGroupPutGroupInGroup'
    public class ResultGroupPutGroupInGroup : ResultBase
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ResultGroupPutGroupInGroup'
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ResultGroupPutGroupInGroup.MyGroup'
        public SINnerGroup MyGroup
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ResultGroupPutGroupInGroup.MyGroup'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ResultGroupPutGroupInGroup.ResultGroupPutGroupInGroup()'
        public ResultGroupPutGroupInGroup() => MyGroup = new SINnerGroup();

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ResultGroupPutGroupInGroup.ResultGroupPutGroupInGroup(SINnerGroup)'
        public ResultGroupPutGroupInGroup(SINnerGroup group) => MyGroup = group;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ResultGroupPutGroupInGroup.ResultGroupPutGroupInGroup(Exception)'
        public ResultGroupPutGroupInGroup(Exception e) : base(e)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ResultGroupPutGroupInGroup.ResultGroupPutGroupInGroup(Exception)'
        {

        }
    }
}
