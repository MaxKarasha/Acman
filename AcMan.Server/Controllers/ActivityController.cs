using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcMan.Server.Core;
using AcMan.Server.Core.DB;
using AcMan.Server.Core.KeyReader;
using AcMan.Server.Integration.SyncStrategy;
using AcMan.Server.Models;
using AcMan.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AcMan.Server.Controllers
{
	public class ActivityController : BaseController<ActivityRepository, Activity>
	{
		public ActivityController(ActivityRepository repository, AcManContext context, ISyncStrategy syncStrategy) : base(repository, context, syncStrategy) { }

		[HttpPost]
        public Activity Continue([FromBody]Activity activity)
		{
            return ContinueById(activity.Id);
        }

        [HttpPost]
        public Activity ContinueById(Guid id)
        {
            ICollection<Activity> activities = Repository.GetByStatus(ActivityStatus.InProgress);
            Activity activity = Repository.Get(id);
            activity.Status = ActivityStatus.InProgress;
            if (activity.Start == null || activity.Start > AcmanHelper.GetCurrentDateTime()) {
                activity.Start = AcmanHelper.GetCurrentDateTime();
            }                
            activities.ToList().ForEach(a => a.Status = ActivityStatus.InPause);
            activities.Add(activity);
            Repository.Edit(activities);
            SyncStrategy.SyncAcmanActivity(activities);
            return activity;
        }

        [HttpPost]
        public Activity Stop([FromBody]Activity activity)
		{
            return StopById(activity.Id);
        }

        [HttpPost]
        public Activity StopById(Guid id)
        {
            var activity = Repository.Get(id);
            activity.Status = ActivityStatus.Done;
            activity.End = AcmanHelper.GetCurrentDateTime();
            if (activity.Start == null) {
                activity.Start = activity.End;
            }
            Repository.Edit(activity);
            SyncStrategy.SyncAcmanActivity(activity);
            return activity;
        }

        [HttpPost]
		public override Guid Add([FromBody]Activity entity)
		{
			if (entity.User == null && entity.UserId == Guid.Empty)
			{
				entity.UserId = CurrentConnection.CurrentUser.Id;
			}
			var id = Repository.Add(entity);
            SyncStrategy.SyncAcmanActivity(entity);
            return id;
        }

		[HttpPost]
        public Guid Start(Activity activity)
		{
			return Add(activity);
		}

		[HttpPost]
		public Guid Create([FromBody]Activity activity)
		{
			return Add(activity);
		}

		[HttpGet]
		public IEnumerable<Activity> GetOnPause()
		{
			return Repository.GetByStatus(new List<ActivityStatus>() { { ActivityStatus.InPause }, { ActivityStatus.New } });
		}

		[HttpGet]
		public IEnumerable<Activity> GetByPeriod(DateTime? startDate, DateTime? endDate)
		{
			return Repository.GetByPeriod(startDate, endDate);
		}
	}
}
