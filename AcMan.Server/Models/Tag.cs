using AcMan.Server.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcMan.Server.Models
{
    [Table("Tag")]
    public class Tag : BaseLookupEntity
    {
        public Guid? ActivityAdditionalInfoId { get; set; }
        public ActivityAdditionalInfo ActivityAdditionalInfo { get; set; }
        public virtual ICollection<TagInActivity> TagInActivities { get; set; }
        [NotMapped]
        public virtual ICollection<Activity> Activities { get; set; }
    }
}
