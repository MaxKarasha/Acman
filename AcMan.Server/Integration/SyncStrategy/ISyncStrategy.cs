using AcMan.Server.Models;
using AcMan.Server.Models.Base;
using AcMan.Server.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Integration.SyncStrategy
{
    public interface ISyncStrategy
    {
        void Sync();
        void SyncUser(User user);
        string Info { get; }
    }
}
