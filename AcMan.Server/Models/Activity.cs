using AcMan.Server.Attribute;
using AcMan.Server.Core;
using AcMan.Server.Models.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models
{
    public enum ActivityStatus
    {
        New,
        InPause,
        InProgress,        
        Done
    }

    [Table("Activity")]
    [RemoteEntityName("Activity")]
    public class Activity : BaseSynchronizedEntityActivityAdditionalInfo
    {
        [RemoteEntityColumnName("Title")]
        public string Caption { get; set; }
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        [RemoteEntityColumnName("OwnerId")]
        public virtual User User { get; set; }
        [RemoteEntityColumnName("StartDate")]
        public DateTime? Start { get; set; }
        [RemoteEntityColumnName("DueDate")]
        public DateTime? End { get; set; }
        [RemoteEntityColumnName("StatusId", "StatusMap")]
        public ActivityStatus Status { get; set; }
        [NotMapped]
        public ICollection<Tag> Tags {
            get => TagInActivities?.Where(p => p.ActivityId == this.Id).Select(p => p.Tag).ToArray();
            set {
                TagInActivities = TagInActivities ?? new Collection<TagInActivity>();
                foreach (var tag in value) {
                    TagInActivities.Add(
                        new TagInActivity {
                            TagId = tag.Id,
                            Tag = tag,
                            ActivityId = Id,
                            Activity = this
                        }
                    );
                }
            }
        }
        public virtual ICollection<TagInActivity> TagInActivities { get; set; }

		public string Source { get; set; }
    }
}
