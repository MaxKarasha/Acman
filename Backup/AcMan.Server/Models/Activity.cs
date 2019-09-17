using AcMan.Server.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models
{
    public enum ActivityStatus
    {
        InProgress,
        InPause,
        Done
    }

    [Table("Activity")]
    public class Activity : IEntity
    {
        public Guid Id { get; set; }
        public string Caption { get; set; }
        public User User { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public ActivityStatus Status { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}
