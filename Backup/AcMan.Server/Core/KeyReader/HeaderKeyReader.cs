using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Core
{
    public class HeaderKeyReader: IKeyReader
    {
        public string GetKeyFromRequest(HttpRequest request)
        {
            return request.Headers[AcmanConstants.KeyName];
        }
    }
}
