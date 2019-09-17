using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models
{
    [Table("TagStrategy")]
    public class TagStrategy : IEntity
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
    }
}
