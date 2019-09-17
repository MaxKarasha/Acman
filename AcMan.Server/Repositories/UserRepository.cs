using AcMan.Server.Core;
using AcMan.Server.Core.DB;
using AcMan.Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Repositories
{
    public class UserRepository : BaseLookupEntityRepository<User>
    {
        public UserRepository(AcManContext context) : base(context) { }

        public User GetByKey(string key)
        {
            return _context.Set<User>().AsNoTracking()
                .Include(c => c.UserInSystems)
                .Where(c => c.UserInSystems.Any(b => b.Key == key))
                .FirstOrDefault();
        }
        public ICollection<User> GetUsersByEndSystem(Guid endSystemId)
        {
            return _context.Set<User>().AsNoTracking()
                .Include(c => c.UserInSystems)
                .Where(c => c.UserInSystems.Any(b => (b.Key != null && b.EndSystemId == endSystemId)))
                .ToList();
        }
    }
}
