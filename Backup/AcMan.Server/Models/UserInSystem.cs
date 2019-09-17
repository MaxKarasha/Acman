using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models
{
    [Table("UserInSystem")]
    public class UserInSystem : IEntity
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public EndSystem EndSystem { get; set; }
        public string Key { get; set; }
    }
}
