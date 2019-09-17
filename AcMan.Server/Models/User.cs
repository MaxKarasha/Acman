using AcMan.Server.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcMan.Server.Models
{
    [Table("User")]
    public class User : BaseSynchronizedLookupEntity
    {
		public string Login { get; set; }
		public string Password { get; set; }
        public ICollection<UserInSystem> UserInSystems { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public DateTime? LastSyncDate { get; set; }
    }
}
