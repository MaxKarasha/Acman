using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcMan.Server.Core;
using AcMan.Server.Core.DB;
using AcMan.Server.Core.KeyReader;
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

        public CurrentActivityController(ActivityRepository repository, AcManContext context) : base(repository, context) {
            CurrentActivity = Repository.GetCurrent();
        }

        [HttpPut]
        public void Pause()
        {
            CurrentActivity.Status = ActivityStatus.InPause;
            Repository.Edit(CurrentActivity);
        }

        [HttpPut]
        public void Stop()
        {
            CurrentActivity.Status = ActivityStatus.Done;
            Repository.Edit(CurrentActivity);
        }
                
        //[HttpGet]
        //public Activity Get()
        //{
        //    //Key;
        //    return null;
        //}

    }
}