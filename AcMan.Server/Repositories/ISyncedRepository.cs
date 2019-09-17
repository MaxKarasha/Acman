using AcMan.Server.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Repositories
{
    interface ISyncedRepository<T> where T : ISynchronizedEntity
    {
        void UpdateRemoteColumns(T entity);
    }
}
