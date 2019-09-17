using AcMan.Server.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models
{
    [Table("WorkInProject")]
    public class WorkInProject : BaseSynchronizedLookupEntity
    {
        [ForeignKey("Project")]
        public Guid? ProjectId { get; set; } = null;
        public Project Project { get; set; }
    }
}
