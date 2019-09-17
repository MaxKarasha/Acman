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
    public class LastActivityController : BaseController<ActivityRepository, Activity>
    {
        public Activity LastActivity { get; set; }

        public LastActivityController(ActivityRepository repository, AcManContext context) : base(repository, context) {
            LastActivity = repository.GetLast();
        }

        [HttpPut]
        public void Continue()
        {
            LastActivity.Id = Guid.NewGuid();
            LastActivity.Start = DateTime.Now;
            LastActivity.Status = ActivityStatus.InPause;
            Repository.Add(LastActivity);
        }

        [HttpPut]
        public void Stop()
        {
            LastActivity.Status = ActivityStatus.Done;
            Repository.Edit(LastActivity);
        }
    }
}
