using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models
{
    [Table("TagType")]
    public class TagType : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TagStrategy TagStrategy { get; set; }
    }
}
