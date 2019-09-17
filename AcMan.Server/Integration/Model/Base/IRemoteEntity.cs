using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Integration.Model.Base
{
    public interface IRemoteEntity
    {
        Dictionary<string, object> Data { get; }
    }
}
