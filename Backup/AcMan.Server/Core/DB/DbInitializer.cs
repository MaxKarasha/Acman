using AcMan.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Core.DB
{
    public static class DbInitializer
    {
        public static void Initialize(AcManContext context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            if (context.Activities.Any())
            {
                return;   // DB has been seeded
            }

            var users = new User[]
            {
                new User("MK", "MK"),
                new User("MK2", "MK2"),
            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();
        }
    }
}
