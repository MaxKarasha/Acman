using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models.Base
{
    public interface ISynchronizedEntity
    {
        Guid? EndSystemRecordId { get; set; }
        bool IsSynchronized { get; set; }
        bool IsIntegration { get; set; }
        bool NeedUpdateRemoteIds { get; set; }
    }
}
