using AcMan.Server.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Integration.RemoteRepository
{
    interface IRemoteRepository<T1> where T1 : class, IEntity
    {
        T1 Get(Guid id);
        Guid Add(T1 entity);
        void Remove(T1 entity);
        void Remove(Guid id);
        void Edit(T1 entity);
        ICollection<T1> GetAll();
    }
}
