using AcMan.Server.Models.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcMan.Server.Models
{
    [Table("TagStrategy")]
    public class TagStrategy : BaseLookupEntity
    {
        public ICollection<TagType> TagTypes { get; set; }
    }
}
