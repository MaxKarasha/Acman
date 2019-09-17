using AcMan.Server.Core.DB;
using AcMan.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Repositories
{
    public class TagInActivityRepository : BaseRepository<TagInActivity>
    {
        public TagInActivityRepository(AcManContext context) : base(context) { }
    }
}
