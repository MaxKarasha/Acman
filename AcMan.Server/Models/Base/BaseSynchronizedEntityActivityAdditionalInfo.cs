using AcMan.Server.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcMan.Server.Models.Base
{
    [NotMapped]
    public abstract class BaseSynchronizedEntityActivityAdditionalInfo : BaseEntityActivityAdditionalInfo, ISynchronizedEntity
    {
        public Guid? EndSystemRecordId { get; set; } = null;
        public bool IsSynchronized { get; set; } = false;
        [NotMapped]
        public bool IsIntegration { get; set; } = false;
        [NotMapped]
        public bool NeedUpdateRemoteIds { get; set; } = false;
    }
}
