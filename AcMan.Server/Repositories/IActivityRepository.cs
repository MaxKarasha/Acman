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
        bool IsIntegration { get; set; }
        Activity GetCurrent();
        Activity GetLast();
        ICollection<Activity> GetByStatus(ActivityStatus status);
        ICollection<Activity> GetByPeriod(DateTime? start, DateTime? end);
        ICollection<Activity> FilterByTags(IEnumerable<Tag> tags);
        ICollection<Activity> FilterByTag(Tag tag);
    }
}
