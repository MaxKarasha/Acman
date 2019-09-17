using AcMan.Server.Core;
using AcMan.Server.Models;
using AcMan.Server.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Controllers
{
    public abstract class BaseController<T1, T2> : Controller where T1 : BaseRepository<T2> where T2 : class, IEntity
    {
        private T1 _repository;
        private IKeyReader _keyReader;
        private string _key;

        public string Key
        {
            get
            {                
                if (string.IsNullOrEmpty(_key))
                {
                    _key = _keyReader?.GetKeyFromRequest(Request) ?? AcmanHelper.GetKeyFromRequest(Request);
                }
                return _key;
            }
        }

        public T1 Repository
        {
            get
            {
                return _repository;
            }
        }

        public BaseController(T1 repository)
        {
            _repository = repository;
        }

        public BaseController(T1 repository, IKeyReader keyReader)
        {
            _repository = repository;
            _keyReader = keyReader;
        }

        [HttpGet]
        public IEnumerable<T2> Get()
        {
            return _repository.GetAll();
        }

        [HttpGet("{id}")]
        public T2 Get(Guid id)
        {
            return _repository.Get(id);
        }

        [HttpPut]
        public void Edit(T2 entity)
        {
            _repository.Edit(entity);
        }

        [HttpPost]
        public Guid Add(T2 entity)
        {
            return _repository.Add(entity);
        }
    }
}
