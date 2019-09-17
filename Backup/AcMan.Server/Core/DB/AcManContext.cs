using AcMan.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcMan.Server.Core.DB
{
	public partial class AcManContext : DbContext
	{
        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserInSystem> ContactInSystems { get; set; }
        public DbSet<EndSystem> EndSystems { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagStrategy> TagStrategies { get; set; }
        public DbSet<TagType> TagTypes { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
        //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory + "/Properties")                
        //        .AddJsonFile("appsettings.json")
        //        .Build();
        //    optionsBuilder.UseNpgsql(configuration.GetConnectionString("AcManConnectionString"));
        }

        public AcManContext(DbContextOptions<AcManContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>().ToTable("Activity");
        }
    }
}
