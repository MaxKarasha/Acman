using AcMan.Server.Attribute;
using AcMan.Server.Core.DB;
using AcMan.Server.Integration.Model.Base;
using AcMan.Server.Integration.OData;
using AcMan.Server.Models;
using AcMan.Server.Models.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AcMan.Server.Integration.Converter
{
    public class BpmOdataConverter : IBaseConverter//<T1, T2> where T1 : IEntity, ISynchronizedEntity where T2 : ODObject
    {
        public BpmOdataConverter() {
        }

        public ICollection<PropertyInfo> GetSyncedProperties(Type t)
        {
            return t.GetProperties()
              .Where(prop =>
                    System.Attribute.IsDefined(prop, typeof(RemoteEntityColumnName))
                ).ToList();
        }

        public ICollection<PropertyInfo> GetSyncedProperties(ISynchronizedEntity baseEntity)
        {
            return GetSyncedProperties(baseEntity.GetType());
        }

        public ICollection<string> GetSyncedRemoteColumns(ICollection<PropertyInfo> p)
        {
            return p
                .Select(s => s.GetCustomAttribute<RemoteEntityColumnName>().Name)
                .ToList();
        }

        public ICollection<string> GetSyncedRemoteColumns(Type t)
        {
            return GetSyncedRemoteColumns(GetSyncedProperties(t));
        }

        public ICollection<string> GetSyncedRemoteColumns(ISynchronizedEntity baseEntity)
        {
            return GetSyncedRemoteColumns(GetSyncedProperties(baseEntity));
        }

        public IRemoteEntity ConvertToRemoteEntity(ISynchronizedEntity baseEntity, ODBase bpm = null)
        {
            var remoteEntityNameAttribute = baseEntity.GetType().GetTypeInfo().GetCustomAttribute<RemoteEntityName>();
            if (remoteEntityNameAttribute == null) {
                throw new Exception("Attribute [RemoteEntityName] not found in class <" + baseEntity.GetType().Name + ">.");
            }
            return ConvertToRemoteEntity(baseEntity, remoteEntityNameAttribute.Name, bpm);
        }

        public IRemoteEntity ConvertToRemoteEntity(ISynchronizedEntity baseEntity, string remoteEntityName, ODBase bpm = null)
        {
            ODObject record = ODObject.NewObject(remoteEntityName);
            if (baseEntity.EndSystemRecordId != null) {
                record.Guid = baseEntity.EndSystemRecordId.ToString();
            }
            var properties = GetSyncedProperties(baseEntity);
            foreach (var prop in properties) {
                var remoteEntityColumnName = prop.GetCustomAttribute<RemoteEntityColumnName>().Name;
                var columnValue = prop.GetValue(baseEntity);
                object result =
                    prop.PropertyType.GetInterface("ISynchronizedEntity") == null ?
                    columnValue :
                    (columnValue as ISynchronizedEntity)?.EndSystemRecordId;
                if (result != null) {
                    record[remoteEntityColumnName] = result;
                }
            }
            return record;
        }

        public ISynchronizedEntity ConvertToAcmanEntity(IRemoteEntity remoteEntity, Type t)
        {
            var properties = GetSyncedProperties(t);
            var entity = Activator.CreateInstance(t) as ISynchronizedEntity;
            entity.NeedUpdateRemoteIds = true;
            entity.EndSystemRecordId = new Guid(remoteEntity.Data["Id"].ToString());
            foreach (var prop in properties) {
                var remoteEntityColumnName = prop.GetCustomAttribute<RemoteEntityColumnName>().Name;
                if (!remoteEntity.Data.ContainsKey(remoteEntityColumnName)) {
                    continue;
                }
                var columnValue = remoteEntity.Data[remoteEntityColumnName];                
                if (columnValue != null) {
                    var propType = prop.PropertyType;
                    if (propType.GetInterface("ISynchronizedEntity") == null) {
                        var typeInfo = propType.GetTypeInfo();
                        if (typeInfo == typeof(DateTime?) || typeInfo == typeof(DateTime)) {
                            prop.SetValue(
                                entity,
                                DateTime
                                    .Parse(columnValue.ToString(), null, DateTimeStyles.RoundtripKind)
                                    .ToLocalTime()
                            );
                        } else {
                            prop.SetValue(entity, columnValue);
                        }
                    } else {
                        var entityIdProp = t.GetProperty(prop.Name + "Id");
                        entityIdProp.SetValue(entity, new Guid(columnValue.ToString()));
                    }
                }
            }
            return entity;
        }
    }
}