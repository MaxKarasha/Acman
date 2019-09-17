using AcMan.Server.Models;
using AcMan.Server.Models.Base;
using System;
using System.Collections.Generic;

namespace AcMan.Server.Repositories
{
    public interface IRepository<T> where T : class, IEntity
    {
        T Get(Guid id);
        Guid Add(T entity);
        void Remove(T entity);
        void Remove(Guid id);
        void Edit(T entity);
        ICollection<T> GetAll();
        ICollection<T> GetAllChangedFrom(DateTime changedOn);
    }
}
