using AcMan.Server.Attribute;
using AcMan.Server.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcMan.Server.Models.Base
{
    [NotMapped]
    public abstract class BaseEntity : IEntity
    {
        [Required]
		[Key]
        public Guid Id { get; set; }
        [RemoteEntityColumnName("CreatedOn")]
        public DateTime CreatedOn { get; set; } = AcmanHelper.GetCurrentDateTime();
        public Guid? CreatedById { get; set; } = null;
        [NotMapped]
        public virtual User CreatedBy { get; set; }
        [RemoteEntityColumnName("ModifiedOn")]
        public DateTime ModifiedOn { get; set; } = AcmanHelper.GetCurrentDateTime();
        public Guid? ModifiedById { get; set; } = null;
        [NotMapped]
        public virtual User ModifiedBy { get; set; }
        [NotMapped]
        public AcmanEntityState EntityState { get; set; } = AcmanEntityState.Unchanged;
    }
}
