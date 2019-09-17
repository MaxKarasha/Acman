using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcMan.Server.Models;
using AcMan.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AcMan.Server.Controllers
{
    [Route("api/[controller]/[Action]")]
    public class LastActivityController : BaseController<ActivityRepository, Activity>
    {
        public LastActivityController(ActivityRepository repository) : base(repository) { }

        [HttpPut]
        public void Continue()
        {
        }

        [HttpPut]
        public void Stop()
        {
        }

        //// GET api/values
        //[HttpGet]
        //public Activity getLastActivity()
        //{
        //    return new Activity();
        //}
    }
}
