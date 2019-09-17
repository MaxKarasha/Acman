using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Core
{
    public class AcmanHelper
    {
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
    }
}
