using AcMan.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        T Get(Guid id);
        Guid Add(T entity);
        void Remove(T entity);
        void Remove(Guid id);
        void Edit(T entity);
        IEnumerable<T> GetAll();
    }
}
