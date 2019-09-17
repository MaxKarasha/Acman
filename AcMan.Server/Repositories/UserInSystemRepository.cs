using AcMan.Server.Core.DB;
using AcMan.Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Repositories
{
    public class UserInSystemRepository : BaseRepository<UserInSystem>
    {
        public UserInSystemRepository(AcManContext context) : base(context) { }

        public UserInSystem GetByKey(string key)
        {
            return _context.Set<UserInSystem>().AsNoTracking()
                .Include(c => c.User).FirstOrDefault(s => s.Key == key);
        }
    }
}
