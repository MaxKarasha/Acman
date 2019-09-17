using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models.Base
{
    public abstract class BaseSynchronizedLookupEntity : BaseLookupEntity, ISynchronizedEntity
    {
        public Guid? EndSystemRecordId { get; set; } = null;
        public bool IsSynchronized { get; set; } = false;
        [NotMapped]
        public bool IsIntegration { get; set; } = false;
        [NotMapped]
        public bool NeedUpdateRemoteIds { get; set; } = false;
    }
}
