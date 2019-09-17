using AcMan.Server.Core.DB;
using AcMan.Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class, IEntity
    {
        public AcManContext _context;

        public BaseRepository(AcManContext context)
        {
            _context = context;
        }

        public T Get(Guid id)
        {
            return _context.Set<T>().AsNoTracking().SingleOrDefault(s => s.Id == id);
        }

        public Guid Add(T entity)
        {
            _context.Set<T>().AddAsync(entity);
            _context.SaveChangesAsync();
            return entity.Id;
        }

        public void Remove(T entity)
        {
            //var entity = _context.Set<T>().FindAsync(id);
            _context.Set<T>().Remove(entity);
            _context.SaveChangesAsync();
        }

        public void Remove(Guid id)
        {
            T entity = _context.Set<T>().FindAsync(id) as T;
            _context.Set<T>().Remove(entity);
            _context.SaveChangesAsync();
        }

        public void Edit(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChangesAsync();
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking();
        }
    }
}
