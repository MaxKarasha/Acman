using AcMan.Server.Core;
using AcMan.Server.Core.DB;
using AcMan.Server.Models;
using AcMan.Server.Models.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        protected AcManContext _context;

        public BaseRepository(AcManContext context)
        {
            _context = context;
        }

        public virtual T Get(Guid id)
        {
            T entity = _context.Set<T>().AsNoTracking().SingleOrDefault(s => s.Id == id);
            if (entity != null)
            {
                entity.EntityState = AcmanEntityState.Unchanged;
            }            
            return entity;
        }

        public virtual Guid Add(T entity)
        {
            entity.EntityState = AcmanEntityState.Added;
            _context.Set<T>().AddAsync(entity);
            var result = _context.SaveChangesAsync();
            result.Wait();
            _context.Entry(entity).State = EntityState.Detached;
            entity.EntityState = AcmanEntityState.Unchanged;
            return entity.Id;
        }

        public void Remove(T entity)
        {
            entity.EntityState = AcmanEntityState.Deleted;
            _context.Set<T>().Remove(entity);
            var result = _context.SaveChangesAsync();
            result.Wait();
        }

        public void Remove(Guid id)
        {
            T entity = Get(id);
            if (entity != null)
            {
                Remove(entity);
            }            
        }

        public virtual void Edit(T entity)
        {
            var local = _context.Set<T>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(entity.Id));
            if (local != null) {
                _context.Entry(local).State = EntityState.Detached;
            }
            entity.EntityState = AcmanEntityState.Modified;
            _context.Set<T>().Update(entity);
            var result = _context.SaveChangesAsync();
            result.Wait();
            _context.Entry(entity).State = EntityState.Detached;
            entity.EntityState = AcmanEntityState.Unchanged;
        }

        public ICollection<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking()
                .ToList()
                .Select(c => { c.EntityState = AcmanEntityState.Unchanged; return c; })
                .ToList();
        }

        public virtual ICollection<T> GetAllChangedFrom(DateTime changedOn)
        {
            return _context.Set<T>().AsNoTracking()
                .Where(s => s.ModifiedOn > changedOn)
                .ToList()
                .Select(c => { c.EntityState = AcmanEntityState.Unchanged; return c; })
                .ToList();
        }
    }
}
