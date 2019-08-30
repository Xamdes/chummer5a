using System;

namespace ChummerHub.Models.V1
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ResultSinnerDelete'
    public class ResultSinnerDelete : ResultBase
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ResultSinnerDelete'
    {
        private bool Deleted
        {
            get; set;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ResultSinnerDelete.ResultSinnerDelete(Exception)'
        public ResultSinnerDelete(Exception e) : base(e)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'ResultSinnerDelete.ResultSinnerDelete(Exception)'
        {

        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ResultSinnerDelete.ResultSinnerDelete(bool)'
        public ResultSinnerDelete(bool deleted) => Deleted = deleted;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'ResultSinnerDelete.ResultSinnerDelete()'
        public ResultSinnerDelete() => Deleted = false;
    }
}
