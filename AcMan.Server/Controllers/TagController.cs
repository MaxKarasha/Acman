using AcMan.Server.Core.DB;
using AcMan.Server.Models;
using AcMan.Server.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Controllers
{
    public class TagController : BaseController<TagRepository, Tag>
    {
        public TagController(TagRepository repository, AcManContext context) : base(repository, context) { }
    }
}
