using AcMan.Server.Core;
using AcMan.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Repositories
{
    interface IActivityRepository
    {
        Activity GetCurrent();
        Activity GetLast();
        IEnumerable<Activity> GetByStatus(ActivityStatus status);
        IEnumerable<Activity> GetByPeriod(DateTime start, DateTime end);
        IEnumerable<Activity> FilterByTag(IEnumerable<Tag> tags);
    }

    
}
