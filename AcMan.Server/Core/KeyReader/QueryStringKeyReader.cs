using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace AcMan.Server.Core.KeyReader
{
    public class QueryStringKeyReader : IKeyReader
    {
        public string GetKeyFromRequest(HttpRequest request)
        {
            if (request.Query.TryGetValue(AcmanConstants.KeyName, out var key)) {
                return key.ToString();
            }
            return null;
        }
    }
}
