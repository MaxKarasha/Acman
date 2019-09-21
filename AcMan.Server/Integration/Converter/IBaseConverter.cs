using AcMan.Server.Core.DB;
using AcMan.Server.Integration.Model.Base;
using AcMan.Server.Integration.OData;
using AcMan.Server.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Integration.Converter
{
    interface IBaseConverter//<T1, T2> where T1 : IEntity, ISynchronizedEntity
    {
        //T1 ConvertToAcmanEntity(T2 remoteEntity);
        //T2 ConvertToRemoteEntity(T1 baseEntity);
        //T2 ConvertToRemoteEntity(T1 baseEntity, string remoteEntityName);
        ISynchronizedEntity ConvertToAcmanEntity(IRemoteEntity remoteEntity, Type t);
        IRemoteEntity ConvertToRemoteEntity(ISynchronizedEntity baseEntity, ODBase bpm);
        IRemoteEntity ConvertToRemoteEntity(ISynchronizedEntity baseEntity, string remoteEntityName, ODBase bpm);
        Dictionary<string, object> GetDefaultEntityValues(string entityName);
    }
}
