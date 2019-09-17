using AcMan.Server.Core.DB;
using AcMan.Server.Core.KeyReader;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Core
{
    public class CurrentConnectionMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentConnectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context, AcManContext dbContext, IKeyReader keyReader)
        {
            CurrentConnection.Key = keyReader?.GetKeyFromRequest(context.Request) ?? AcmanHelper.GetKeyFromRequest(context.Request);
            //TODO: remove this
            if (System.Diagnostics.Debugger.IsAttached && string.IsNullOrEmpty(CurrentConnection.Key)) {
                CurrentConnection.Key = "TestMKKey";
            }
            if (!string.IsNullOrEmpty(CurrentConnection.Key))
            {
                CurrentConnection.CurrentUser = AcmanHelper.GetUserByKey(CurrentConnection.Key, dbContext);
            }            
            return this._next(context);
        }
    }
}
