using AcMan.Server.Models.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcMan.Server.Models
{
    [Table("UserInSystem")]
    public class UserInSystem : BaseSynchronizedEntity
    {
		[ForeignKey("User")]
		public Guid UserId { get; set; }
        [Required]
        public virtual User User { get; set; }
		[ForeignKey("EndSystem")]
		public Guid? EndSystemId { get; set; } = null;
        public EndSystem EndSystem { get; set; }
        [Required]
        public string Key { get; set; }
    }
}
