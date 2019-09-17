using AcMan.Server.Core.DB;
using AcMan.Server.Models.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Repositories
{
    public class BaseLookupEntityRepository<T> : BaseRepository<T> where T : BaseLookupEntity, IEntity
    {
        public BaseLookupEntityRepository(AcManContext context) : base(context) { }
        public T GetByName(string name)
        {
            T entity = _context.Set<T>().AsNoTracking().SingleOrDefault(s => s.Name == name);
            if (entity != null) {
                entity.EntityState = AcmanEntityState.Unchanged;
            }
            return entity;
        }

        public T GetByName(Enum enumValue)
        {
            return GetByName(enumValue.ToString());
        }
    }
}
