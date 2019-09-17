using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcMan.Server.Core;
using AcMan.Server.Integration.SyncStrategy;
using Microsoft.AspNetCore.Mvc;

namespace AcMan.Server.Controllers
{
    [Route("api/[controller]/[Action]")]
    public class SynchronizationController : Controller
    {
        private ISyncStrategy _syncStrategy;

        public SynchronizationController(ISyncStrategy syncStrategy)
        {
            _syncStrategy = syncStrategy;
        }

        [HttpGet]
        public IActionResult Sync()
        {
            _syncStrategy.Sync();
            return Ok(_syncStrategy.Info);
        }

        [HttpGet]
        public IActionResult SyncMe()
        {
            _syncStrategy.SyncUser(CurrentConnection.CurrentUser);
            return Ok(_syncStrategy.Info);
        }
    }
}