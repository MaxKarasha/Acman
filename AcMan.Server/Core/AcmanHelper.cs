using AcMan.Server.Attribute;
using AcMan.Server.Core.DB;
using AcMan.Server.Core.KeyReader;
using AcMan.Server.Models;
using AcMan.Server.Models.Base;
using AcMan.Server.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Core
{
    public class AcmanHelper
    {
        public static DateTime GetCurrentDateTime() { return DateTime.Now; }

        public static User GetUserByKey(string key, AcManContext context)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            var userRepository = new UserRepository(context);
            User user = userRepository.GetByKey(key);
            if (user == null)
            {                
                user = new User
                {
                    Name = "Autogenerate from key: " + key,
                    Login = "Autogenerate " + key
                };
                userRepository.Add(user);
                var userInSystem = new UserInSystem
                {
                    User = user,
                    Key = key
                };
                var userInSystemRepository = new UserInSystemRepository(context);
                userInSystemRepository.Add(userInSystem);
            }            
            return user;
        }

        public static User GetUserFromRequest(HttpRequest request, AcManContext context)
        {
            var key = GetKeyFromRequest(request);
            return GetUserByKey(key, context);
        }

        public static User GetUserFromRequest(HttpRequest request)
        {
            var context = request.HttpContext.RequestServices.GetRequiredService<AcManContext>();
            return GetUserFromRequest(request, context);
        }

        public static string GetKeyFromRequest(HttpRequest request)
        {
            var iKeyReaderType = typeof(IKeyReader);
            var iKeyReaderTypes = 
                iKeyReaderType
                    .Assembly
                    .GetTypes()
                    .Where(x => iKeyReaderType.IsAssignableFrom(x) && x.IsClass)
                    .Select(x => Activator.CreateInstance(x) as IKeyReader)
                    .ToArray();
            foreach (var keyReader in iKeyReaderTypes)
            {
                var key = keyReader.GetKeyFromRequest(request);
                if (!string.IsNullOrEmpty(key))
                {
                    return key;
                }
            }
            return null;
        }

        public static EntityState ConvertToEntityState(AcmanEntityState objectState)
        {
            EntityState efState = EntityState.Unchanged;
            switch (objectState)
            {
                case AcmanEntityState.Added:
                    efState = EntityState.Added;
                    break;
                case AcmanEntityState.Modified:
                    efState = EntityState.Modified;
                    break;
                case AcmanEntityState.Deleted:
                    efState = EntityState.Deleted;
                    break;
            }
            return efState;
        }

        public static void MergeSyncedObjects<T>(T target, T source)
        {
            Type t = typeof(T);
            var properties = t.GetProperties().Where(
                prop => System.Attribute.IsDefined(prop, typeof(RemoteEntityColumnName))
            );
            foreach (var prop in properties) {
                var propType = prop.PropertyType;
                if (propType.GetInterface("ISynchronizedEntity") == null) {
                    var newValue = prop.GetValue(source);
                    var oldValue = prop.GetValue(target);
                    if (newValue != null && (oldValue == null || newValue != oldValue)) {
                        prop.SetValue(target, newValue, null);
                    }
                } else {
                    var entityIdProp = t.GetProperty(prop.Name + "Id");
                    var value = entityIdProp.GetValue(source);
                    if (value != null) {
                        entityIdProp.SetValue(target, new Guid(value.ToString()));
                    }                    
                }                 
            }
        }
    }
}
