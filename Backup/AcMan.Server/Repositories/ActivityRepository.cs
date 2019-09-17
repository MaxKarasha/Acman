using AcMan.Server.Core;
using AcMan.Server.Core.DB;
using AcMan.Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Repositories
{
    public class ActivityRepository : BaseRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(AcManContext context) : base(context) {}
        
        public IEnumerable<Activity> FilterByTag(IEnumerable<Tag> tags)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Activity> GetByPeriod(DateTime start, DateTime end)
        {
            return _context.Activities.Where(s => s.Start >= start && s.Start <= end);
        }

        public IEnumerable<Activity> GetByStatus(ActivityStatus status)
        {
            return _context.Activities.Where(s => s.Status == status);
        }

        public Activity GetCurrent()
        {
            return _context.Activities.SingleOrDefault(s => s.Status == ActivityStatus.InProgress);
        }

        public Activity GetLast()
        {
            return _context.Activities.Where(s => s.Start <= DateTime.Now)
                    .OrderByDescending(s => s.Start)
                    .FirstOrDefault();
        }
    }
}
