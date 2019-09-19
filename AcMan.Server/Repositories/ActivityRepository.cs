using AcMan.Server.Core;
using AcMan.Server.Core.DB;
using AcMan.Server.Models;
using AcMan.Server.Models.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Repositories
{
    public class ActivityRepository : BaseSyncedRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(AcManContext context) : base(context) { }
        
        public ICollection<Activity> FilterByTags(IEnumerable<Tag> tags)
        {
            //TODO
            return _context.Activities.AsNoTracking()
                //.Include(c => c.TagInActivities.Where(s => tags.Contains(s.Tag)))
                .Include(c => c.TagInActivities)
                .ThenInclude(t => t.Tag)
                //.Where(s => s.TagInActivities)
                .ToList();
        }

        public ICollection<Activity> FilterByTag(Tag tag)
        {
            //TODO
            return _context.Activities.AsNoTracking()
                .Include(c => c.TagInActivities.Where(s => s.Tag == tag))
                .ThenInclude(t => t.Tag)
                .ToList();
        }

        public ICollection<Activity> GetByPeriod(DateTime? start, DateTime? end)
        {
            return _context.Activities
                .Where(s => (!start.HasValue || s.CreatedOn >= start) && (!end.HasValue || s.CreatedOn <= end))
                .ToList();
        }

        public ICollection<Activity> GetByStatus(ActivityStatus status)
        {
            return _context.Activities.Where(s => s.Status == status).ToList();
        }

        public override Guid Add(Activity entity)
        {
            entity.EntityState = AcmanEntityState.Added;
            entity.IsIntegration = IsIntegration;
            if (entity.NeedUpdateRemoteIds) {
                UpdateRemoteColumns(entity);
                entity.NeedUpdateRemoteIds = false;
            }
            _context.Set<Activity>().AddAsync(entity);
            if (entity.TagInActivities != null) {
                foreach (var tagInActivity in entity.TagInActivities) {
                    tagInActivity.EntityState = AcmanEntityState.Added;
                    _context.Set<TagInActivity>().AddAsync(tagInActivity);
                }
            }
            var result = _context.SaveChangesAsync();
            result.Wait();
            _context.Entry(entity).State = EntityState.Detached;
            entity.EntityState = AcmanEntityState.Unchanged;
            return entity.Id;
        }

        public override ICollection<Activity> GetAllChangedFrom(DateTime changedOn)
        {
            return _context.Set<Activity>().AsNoTracking()
                .Include(c => c.TagInActivities)
                .ThenInclude(t => t.Tag)
                .Where(s => s.ModifiedOn > changedOn)
                .ToList()
                .Select(c => { c.EntityState = AcmanEntityState.Unchanged; return c; })
                .ToList();
        }

        public ICollection<Activity> GetAllChangedBetweenDatesForUser(DateTime startDate, DateTime endDate, User user)
        {
            return _context.Set<Activity>().AsNoTracking()
                .Include(c => c.User)
                .Include(c => c.Account)
                .Include(c => c.Project)
                .Include(c => c.WorkInProject)
                .Include(c => c.TagInActivities)
                .ThenInclude(t => t.Tag)
                .Where(s => (
                    s.ModifiedOn >= startDate &&
                    s.ModifiedOn < endDate &&
                    s.UserId == user.Id
                ))
                .ToList()
                .Select(c => { c.EntityState = AcmanEntityState.Unchanged; return c; })
                .ToList();
        }

        public ICollection<Activity> GetUnsyncedForUser(User user)
        {
            return _context.Set<Activity>().AsNoTracking()
                .Include(c => c.User)
                .Include(c => c.Account)
                .Include(c => c.Project)
                .Include(c => c.WorkInProject)
                .Include(c => c.TagInActivities)
                .ThenInclude(t => t.Tag)
                .Where(s => (
                    s.IsSynchronized == false &&
                    s.UserId == user.Id
                ))
                .ToList()
                .Select(c => { c.EntityState = AcmanEntityState.Unchanged; return c; })
                .ToList();
        }

        public override void UpdateRemoteColumns(Activity entity)
        {
            //TODO: repeaters! Change it.
            var remoteUserId = entity.UserId;
            if (remoteUserId != Guid.Empty) {
                Guid? resultId = _context.Set<User>().AsNoTracking()
                    .Where(u => u.EndSystemRecordId == remoteUserId)
                    .Select(u => u.Id)
                    .FirstOrDefault();
                entity.UserId = resultId != Guid.Empty ? resultId : null;
            }
            var remoteAccountId = entity.AccountId;
            if (remoteAccountId.HasValue && remoteAccountId != Guid.Empty) {
                Guid? resultId = _context.Set<Account>().AsNoTracking()
                    .Where(u => u.EndSystemRecordId == remoteAccountId)
                    .Select(u => u.Id)
                    .FirstOrDefault();
                entity.AccountId = resultId != Guid.Empty ? resultId : null;
            }
            var remoteProjectId = entity.ProjectId;
            if (remoteProjectId.HasValue && remoteProjectId != Guid.Empty) {                
                Guid? resultId = _context.Set<Project>().AsNoTracking()
                    .Where(u => u.EndSystemRecordId == remoteProjectId)
                    .Select(u => u.Id)
                    .FirstOrDefault();
                entity.ProjectId = resultId != Guid.Empty ? resultId : null;
            }
            var remoteWorkInProjectId = entity.WorkInProjectId;
            if (remoteWorkInProjectId.HasValue && remoteWorkInProjectId != Guid.Empty) {
                Guid? resultId = _context.Set<WorkInProject>().AsNoTracking()
                    .Where(u => u.EndSystemRecordId == remoteWorkInProjectId)
                    .Select(u => u.Id)
                    .FirstOrDefault();
                entity.WorkInProjectId = resultId != Guid.Empty ? resultId : null;
            }
        }

        public Activity GetWithRelation(Guid id)
        {
            Activity entity = _context.Set<Activity>()
                .AsNoTracking()
                .Include(c => c.User)
                .Include(c => c.Account)
                .Include(c => c.Project)
                .Include(c => c.WorkInProject)
                .Include(c => c.TagInActivities)
                .ThenInclude(t => t.Tag)
                .SingleOrDefault(s => s.Id == id);
            if (entity != null) {
                entity.EntityState = AcmanEntityState.Unchanged;
            }
            return entity;
        }

        public Activity GetByRemoteRecordId(Guid? remoteRecordId)
        {
            if (remoteRecordId == null) {
                return null;
            }
            Activity entity = _context.Set<Activity>()
                .AsNoTracking()
                .Include(c => c.User)
                .Include(c => c.Account)
                .Include(c => c.Project)
                .Include(c => c.WorkInProject)
                .Include(c => c.TagInActivities)
                .ThenInclude(t => t.Tag)
                .SingleOrDefault(s => s.EndSystemRecordId == remoteRecordId);
            if (entity != null) {
                entity.EntityState = AcmanEntityState.Unchanged;
            }
            return entity;
        }

        public override Activity Get(Guid id)
        {
            return GetWithTags(id);
        }

        public Activity GetWithTags(Guid id)
        {
            Activity entity = _context.Set<Activity>()
                .AsNoTracking()
                .Include(c => c.TagInActivities)
                .ThenInclude(t => t.Tag)
                .SingleOrDefault(s => s.Id == id);
            if (entity != null) {
                entity.EntityState = AcmanEntityState.Unchanged;
            }
            return entity;
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
