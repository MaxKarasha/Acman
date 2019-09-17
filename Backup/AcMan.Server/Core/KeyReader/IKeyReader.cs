using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Core
{
    public interface IKeyReader
    {
        string GetKeyFromRequest(HttpRequest request);
    }
}
