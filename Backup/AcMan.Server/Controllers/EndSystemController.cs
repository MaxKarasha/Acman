using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcMan.Server.Core;
using AcMan.Server.Models;
using AcMan.Server.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AcMan.Server.Controllers
{
    public class EndSystemController : BaseController<EndSystemRepository, EndSystem>
    {
        public EndSystemController(EndSystemRepository repository, IKeyReader keyReader) : base(repository, keyReader) { }
    }
}
