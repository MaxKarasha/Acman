using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcMan.Server.Core;
using AcMan.Server.Core.DB;
using AcMan.Server.Core.KeyReader;
using AcMan.Server.Models;
using AcMan.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AcMan.Server.Controllers
{
	public class ActivityController : BaseController<ActivityRepository, Activity>
	{
		public ActivityController(ActivityRepository repository, AcManContext context) : base(repository, context) { }

		[HttpPost]
        public void Continue(Activity activity)
		{
			activity.Status = ActivityStatus.InProgress;
			Repository.Edit(activity);
		}

		[HttpPut]
        public void Stop(Activity activity)
		{
			activity.Status = ActivityStatus.Done;
			Repository.Edit(activity);
		}

		[HttpPost]
		public override Guid Add([FromBody]Activity entity)
		{
			if (entity.User == null && entity.UserId == Guid.Empty)
			{
				entity.UserId = CurrentConnection.CurrentUser.Id;
			}
			return Repository.Add(entity);
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
			return Repository.GetByStatus(ActivityStatus.InPause);
		}

		[HttpGet]
		public IEnumerable<Activity> GetByPeriod(DateTime? startDate, DateTime? endDate)
		{
			return Repository.GetByPeriod(startDate, endDate);
		}
	}
}
