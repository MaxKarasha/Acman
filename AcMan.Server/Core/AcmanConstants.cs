using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Core
{
    public static class AcmanConstants
    {
        public const string KeyName = "AcmanKey";

        
        public static class User
        {
            public static Guid AcmanSUId = new Guid("4845097F-57A8-43C6-A189-CD9E9B243701");
        }

        public static class EndSystem
        {
            public static Guid BpmonlineWorkTsi = new Guid("5759DE15-D0F2-4D5C-8AB4-6178F92B2714");
        }
    }
}
