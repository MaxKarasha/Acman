using AcMan.Server.Attribute;
using AcMan.Server.Integration.Converter;
using AcMan.Server.Integration.OData;
using AcMan.Server.Models;
using AcMan.Server.Models.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AcMan.Server.Integration.RemoteRepository
{
    public class BaseBpmOdataRepository<T> : IRemoteRepository<T> where T : class, IEntity, ISynchronizedEntity
    {
        protected ODBase _context;
        protected BpmOdataConverter _bpmOdataConverter;
        protected string _collectionName;
        public BaseBpmOdataRepository(ODBase context, BpmOdataConverter bpmOdataConverter)
        {
            var remoteEntityNameAttribute = typeof(T).GetTypeInfo().GetCustomAttribute<RemoteEntityName>();
            if (remoteEntityNameAttribute == null) {
                throw new Exception("Attribute [RemoteEntityName] not found in class <" + typeof(T).Name + ">.");                
            }
            _collectionName = remoteEntityNameAttribute.Name;
            _context = context;
            _bpmOdataConverter = bpmOdataConverter;
        }
        public Guid Add(T entity)
        {
            var remoteEntity = (_bpmOdataConverter.ConvertToRemoteEntity(entity, _collectionName) as ODObject);
            var result = remoteEntity.Update(_context);
            return ODObject.GetGuidFromEntityUrl(result, _collectionName);
        }

        public void Edit(T entity)
        {
            var remoteEntity = (_bpmOdataConverter.ConvertToRemoteEntity(entity, _collectionName, _context) as ODObject);
            remoteEntity.Guid = entity.EndSystemRecordId.ToString();
            remoteEntity.Update(_context);
        }

        public T Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetAllChangedBetweenDatesForUser(DateTime startDate, DateTime endDate, User user)
        {
            //$select = Id,
            var resultCollection = new Collection<T>();
            var remoteColumns = _bpmOdataConverter.GetSyncedRemoteColumns(typeof(T));
            if (!remoteColumns.Contains("Id")) {
                remoteColumns.Add("Id");
            }
            string selectColumns = "$select=" + string.Join(",", remoteColumns);
            string filter = "$filter=Owner/Id eq guid'" + user.EndSystemRecordId.ToString() + "' " +
                "and ModifiedOn ge datetime'" + ODBase.DateTimeToOdataString(startDate) + "' " +
                "and ModifiedOn lt datetime'" + ODBase.DateTimeToOdataString(endDate) + "'";
            var result = _context.GetAllPages(_collectionName, selectColumns + "&" + filter);
            foreach (var entry in result) {
                ODObject item = ODBase.getObjectFromEntry(_collectionName, entry);
                resultCollection.Add(_bpmOdataConverter.ConvertToAcmanEntity(item, typeof(T)) as T);
            }
            return resultCollection;
        }

        public void Remove(T entity)
        {
            Remove(entity.EndSystemRecordId ?? Guid.Empty);
        }

        public void Remove(Guid id)
        {
            _context.DeleteItem(_collectionName, id.ToString());
        }
    }
}
