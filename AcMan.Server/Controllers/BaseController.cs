using AcMan.Server.Core;
using AcMan.Server.Core.DB;
using AcMan.Server.Core.KeyReader;
using AcMan.Server.Models;
using AcMan.Server.Models.Base;
using AcMan.Server.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Controllers
{
    [Route("api/[controller]/[Action]")]
    public abstract class BaseController<T1, T2> : Controller where T1 : BaseRepository<T2> where T2 : class, IEntity
	{
		private T1 _repository;
        private AcManContext _context;

        public T1 Repository {
			get {
				return _repository;
			}
		}

        public BaseController(T1 repository)
		{
			_repository = repository;
		}

        public BaseController(T1 repository, AcManContext context)
		{
			_repository = repository;
            _context = context;
		}

		[HttpGet]
		public IEnumerable<T2> Get()
		{
			return _repository.GetAll();
		}

		[HttpGet("{id}")]
		public virtual T2 Get(Guid id)
		{
			return _repository.Get(id);
		}

		[HttpPut]
		public void Edit([FromBody]T2 entity)
		{
			_repository.Edit(entity);
		}

		[HttpPost]
		public virtual Guid Add([FromBody]T2 entity)
		{
            return _repository.Add(entity);
		}

        [HttpDelete]
        public virtual void Remove([FromBody]T2 entity)
        {
            _repository.Remove(entity);
        }

        [HttpDelete("{id}")]
        public virtual void Remove(Guid id)
        {
            _repository.Remove(id);
        }
    }
}
