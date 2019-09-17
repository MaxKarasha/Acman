using AcMan.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Core
{
    public class CurrentConnection
    {
        public static User CurrentUser { get; set; }

        public static string Key { get; set; }
    }
}
