using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models
{
    [Table("Tag")]
    public class Tag : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TagStrategy Type { get; set; }
    }
}
