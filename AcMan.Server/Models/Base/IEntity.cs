using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models.Base
{
    public enum AcmanEntityState
    {
        Unchanged,
        Added,
        Modified,
        Deleted
    }

    public interface IEntity
    {
        Guid Id { get; set; }
        DateTime CreatedOn { get; set; }
        Guid? CreatedById { get; set; }
        User CreatedBy { get; set; }
        DateTime ModifiedOn { get; set; }
        Guid? ModifiedById { get; set; }
        User ModifiedBy { get; set; }
        AcmanEntityState EntityState { get; set; }
    }
}
