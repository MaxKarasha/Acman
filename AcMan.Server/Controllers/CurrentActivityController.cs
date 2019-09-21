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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AcMan.Server.Controllers
{
    public class CurrentActivityController : BaseController<ActivityRepository, Activity>
    {
        public Activity CurrentActivity { get; set; }

        public CurrentActivityController(ActivityRepository repository, AcManContext context, ISyncStrategy syncStrategy) : base(repository, context, syncStrategy) {
            CurrentActivity = Repository.GetCurrent();
        }

        [HttpPost]
        public Activity Pause()
        {
            CurrentActivity.Status = ActivityStatus.InPause;
            CurrentActivity.End = AcmanHelper.GetCurrentDateTime();
            Repository.Edit(CurrentActivity);
            SyncStrategy.SyncAcmanActivity(CurrentActivity);
            return CurrentActivity;
        }

        [HttpPost]
        public Activity Stop()
        {
            CurrentActivity.Status = ActivityStatus.Done;
            CurrentActivity.End = AcmanHelper.GetCurrentDateTime();
            Repository.Edit(CurrentActivity);
            SyncStrategy.SyncAcmanActivity(CurrentActivity);
            return CurrentActivity;
        }

        [HttpGet]
        public Activity GetCurrent()
        {
            return CurrentActivity;
        }
    }
}