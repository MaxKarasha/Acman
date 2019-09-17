using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Attribute
{
    public class RemoteEntityColumnName : System.Attribute
    {
        public string Name;
        public RemoteEntityColumnName(string name)
        {
            Name = name;
        }
    }
}
