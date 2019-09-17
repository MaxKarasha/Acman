using AcMan.Server.Core.DB;
using AcMan.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Repositories
{
    public class EndSystemRepository : BaseLookupEntityRepository<EndSystem>
    {
        public EndSystemRepository(AcManContext context) : base(context) { }
    }
}
