using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcMan.Server.Core;
using AcMan.Server.Models;
using AcMan.Server.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AcMan.Server.Controllers
{
    [Route("api/[controller]/[Action]")]
    public class CurrentActivityController : BaseController<ActivityRepository, Activity>
    {
        public CurrentActivityController(ActivityRepository repository, IKeyReader keyReader) : base(repository, keyReader) { }

        [HttpPut]
        public void Pause()
        {
        }

        [HttpPut]
        public void Stop()
        {
        }
                
        //[HttpGet]
        //public Activity Get()
        //{
        //    //Key;
        //    return null;
        //}

    }
}