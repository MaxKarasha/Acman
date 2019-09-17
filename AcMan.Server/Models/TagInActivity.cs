using AcMan.Server.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models
{
	[Table("TagInActivity")]
	public class TagInActivity : BaseEntity
	{
		public Guid ActivityId { get; set; }
		public Guid TagId { get; set; }
        [NotMapped]
		public virtual Activity Activity { get; set; }
        [NotMapped]
        public virtual Tag Tag { get; set; }
	}
}
