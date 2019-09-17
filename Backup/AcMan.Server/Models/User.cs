using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Models
{
    [Table("User")]
    public class User : IEntity
    {
		public Guid Id { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }

        public User() {}

		public User(string login, string pass) {
			this.Id = Guid.NewGuid();
			this.Login = login;
			this.Password = pass;
		}
	}
}
