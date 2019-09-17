using AcMan.Server.Models;
using AcMan.Server.Models.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AcMan.Server.Core.DB
{
	public partial class AcManContext : DbContext
	{
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<ActivityAdditionalInfo> ActivityAdditionalInfos { get; set; }
        public DbSet<EndSystem> EndSystems { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagInActivity> TagInActivities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserInSystem> UserInSystems { get; set; }
        public DbSet<WorkInProject> WorkInProjects { get; set; }
        public DbSet<Synchronization> Synchronizations { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
        }

        public AcManContext()
        {
        }

        public AcManContext(DbContextOptions<AcManContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>()
                .HasOne(x => x.Account)
                .WithMany()
                .HasForeignKey(x => x.AccountId);

            modelBuilder.Entity<Activity>()
                .HasOne(x => x.WorkInProject)
                .WithMany()
                .HasForeignKey(x => x.WorkInProjectId);

            modelBuilder.Entity<Activity>()
                .HasOne(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.ProjectId);

            modelBuilder.Entity<ActivityAdditionalInfo>()
                .HasOne(x => x.Account)
                .WithMany()
                .HasForeignKey(x => x.AccountId);

            modelBuilder.Entity<ActivityAdditionalInfo>()
                .HasOne(x => x.WorkInProject)
                .WithMany()
                .HasForeignKey(x => x.WorkInProjectId);

            modelBuilder.Entity<ActivityAdditionalInfo>()
                .HasOne(x => x.Project)
                .WithMany()
                .HasForeignKey(x => x.ProjectId);

            modelBuilder.Entity<User>()
                .HasMany(c => c.UserInSystems)
                .WithOne(e => e.User)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Tag>()
                .HasOne(c => c.ActivityAdditionalInfo)
                .WithMany(e => e.Tags);

            modelBuilder.Entity<Activity>()                
                .HasOne(x => x.User)
				.WithMany(x => x.Activities)
				.HasForeignKey(x => x.UserId);

            modelBuilder.Entity<TagInActivity>()
				.HasOne(x => x.Activity)
                .WithMany(x => x.TagInActivities)
                .HasForeignKey(x => x.ActivityId);

            modelBuilder.Entity<TagInActivity>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.TagInActivities)
                .HasForeignKey(x => x.TagId);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ProcessChanges();
            return (await base.SaveChangesAsync(true, cancellationToken));
        }

        public override int SaveChanges()
        {
            ProcessChanges();            
            return base.SaveChanges();
        }

        public void ProcessChanges()
        {
            var currentUserId = CurrentConnection.CurrentUser?.Id ?? AcmanConstants.User.AcmanSUId;
            var now = AcmanHelper.GetCurrentDateTime();

            var addedEntries = ChangeTracker.Entries()
                    .Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in addedEntries)
            {
                var entity = entry.Entity as IEntity;
                bool isIntegration = false;
                if (entry.Entity.GetType().GetInterface("ISynchronizedEntity") != null) {
                    isIntegration = (entry.Entity as ISynchronizedEntity).IsIntegration;
                    (entry.Entity as ISynchronizedEntity).IsSynchronized = isIntegration;
                }
                entry.State = AcmanHelper.ConvertToEntityState(entity.EntityState);
                if (entry.State == EntityState.Added) {
                    entity.CreatedById = currentUserId;
                    entity.CreatedOn = now;
                }
                if (entry.State != EntityState.Unchanged) {
                    entity.ModifiedById = currentUserId;
                    entity.ModifiedOn = now;
                }                               
            }
        }
    }
}
