using AcMan.Server.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models
{
    [Table("Synchronization")]
    public class Synchronization : BaseEntity
    {
        public DateTime StartPeriod { get; set; }
        public DateTime EndPeriod { get; set; }
        public int Duration { get; set; }
        [ForeignKey("EndSystem")]
        public Guid? EndSystemId { get; set; } = null;
        public EndSystem EndSystem { get; set; }
        public string Info { get; set; }
    }
}
