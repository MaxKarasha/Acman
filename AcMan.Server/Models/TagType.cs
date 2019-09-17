using AcMan.Server.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcMan.Server.Models
{
    [Table("TagType")]
    public class TagType : BaseLookupEntity
    {
        public TagStrategy TagStrategy { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
