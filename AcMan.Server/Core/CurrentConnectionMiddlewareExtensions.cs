using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Core
{
    public static class CurrentConnectionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCurrentConnection(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CurrentConnectionMiddleware>();
        }
    }
}
