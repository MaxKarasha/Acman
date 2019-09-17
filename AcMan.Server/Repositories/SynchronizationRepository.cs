using AcMan.Server.Core;
using AcMan.Server.Core.DB;
using AcMan.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Repositories
{
    public class SynchronizationRepository : BaseRepository<Synchronization>
    {
        public SynchronizationRepository(AcManContext context) : base(context) { }

        public DateTime GetLastSyncDate()
        {
            return
                _context.Synchronizations.Any() ?
                _context.Synchronizations.Max(p => p.EndPeriod) :
                AcmanHelper.GetCurrentDateTime().AddDays(-7);
        }
    }
}
