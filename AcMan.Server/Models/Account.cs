using AcMan.Server.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models
{
    [Table("Account")]
    public class Account : BaseSynchronizedLookupEntity
    {
    }
}
