//using Swashbuckle.AspNetCore.Filters;
using ChummerHub.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Serialization;

namespace ChummerHub.Models.V1
{
    [DebuggerDisplay("SINner {Id}")]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINner'
    public class SINner : SINnerUploadAble
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINner'
    {

        [MaxLength(2)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINner.EditionNumber'
        public string EditionNumber
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINner.EditionNumber'


        [JsonIgnore]
        [XmlIgnore]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINner.UploadClientId'
        public Guid UploadClientId
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINner.UploadClientId'

        [JsonIgnore]
        [XmlIgnore]
        [MaxLength(8)]
        internal string Hash
        {
            get; set;
        }

        [NotMapped]
        [MaxLength(8)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINner.MyHash'
        public string MyHash
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINner.MyHash'
        {
            get
            {
                if (string.IsNullOrEmpty(Hash))
                    Hash = string.Format("{0:X}", this.Id.ToString().GetHashCode());
                return Hash;
            }
            set
            {
                Hash = value;
            }
        }

        [MaxLength(6)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINner.Language'
        public string Language
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINner.Language'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINner.SINnerMetaData'
        public SINnerMetaData SINnerMetaData
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINner.SINnerMetaData'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINner.LastDownload'
        public DateTime? LastDownload
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINner.LastDownload'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINner.MyGroup'
        public SINnerGroup MyGroup
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINner.MyGroup'

        [MaxLength(64)]
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINner.Alias'
        public string Alias
        {
            get; set;
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINner.Alias'

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'SINner.SINner()'
        public SINner()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'SINner.SINner()'
        {
            Id = Guid.NewGuid();
            this.SINnerMetaData = new SINnerMetaData();
            //this.MyExtendedAttributes = new SINnerExtended(this);
            this.DownloadUrl = "";
            this.MyGroup = null;
            this.Language = "";
            EditionNumber = "5e";
        }

        internal static async Task<List<SINner>> GetSINnersFromUser(ApplicationUser user, ApplicationDbContext context, bool canEdit)
        {
            using (TransactionScope t = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }, TransactionScopeAsyncFlowOption.Enabled))
            {
                List<SINner> result = new List<SINner>();
                List<Guid?> userseq = await (from a in context.UserRights
                                             where a.EMail == user.NormalizedEmail && a.CanEdit == canEdit
                                             select a.SINnerId).ToListAsync();
                List<SINner> sinseq = await context.SINners
                    .Include(a => a.MyGroup)
                    .Where(a => userseq.Contains(a.Id)).ToListAsync();
                t.Complete();
                return sinseq;
            }
        }
    }
}
