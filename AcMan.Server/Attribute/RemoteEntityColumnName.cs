using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Attribute
{
    public class RemoteEntityColumnName : System.Attribute
    {
        public string Name;
        public string MapKey;
        public RemoteEntityColumnName(string name)
        {
            Name = name;
        }
        public RemoteEntityColumnName(string name, string key)
        {
            Name = name;
            MapKey = key;
        }
    }
}
