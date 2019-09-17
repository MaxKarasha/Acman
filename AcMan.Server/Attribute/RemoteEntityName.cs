using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Attribute
{
    public class RemoteEntityName : System.Attribute
    {
        public string Name;
        public RemoteEntityName(string name)
        {
            Name = name;
        }
    }
}
