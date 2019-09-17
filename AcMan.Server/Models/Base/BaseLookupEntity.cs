using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models.Base
{
    public abstract class BaseLookupEntity: BaseEntity
    {
        public string Name { get; set; }
    }
}
