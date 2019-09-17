using AcMan.Server.Core.DB;
using AcMan.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(AcManContext context) : base(context) { }
    }
}
