﻿// <auto-generated />
using AcMan.Server.Core.DB;
using AcMan.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace AcMan.Server.Migrations
{
    [DbContext(typeof(AcManContext))]
    partial class AcManContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AcMan.Server.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid?>("EndSystemRecordId");

                    b.Property<bool>("IsSynchronized");

                    b.Property<Guid?>("ModifiedById");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("AcMan.Server.Models.Activity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("AccountId");

                    b.Property<string>("Caption");

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("End");

                    b.Property<Guid?>("EndSystemRecordId");

                    b.Property<bool>("IsSynchronized");

                    b.Property<string>("JiraUrl");

                    b.Property<Guid?>("ModifiedById");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<Guid?>("ProjectId");

                    b.Property<string>("Source");

                    b.Property<DateTime?>("Start");

                    b.Property<int>("Status");

                    b.Property<Guid?>("UserId");

                    b.Property<Guid?>("WorkInProjectId");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("UserId");

                    b.HasIndex("WorkInProjectId");

                    b.ToTable("Activity");
                });

            modelBuilder.Entity("AcMan.Server.Models.ActivityAdditionalInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("AccountId");

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("JiraUrl");

                    b.Property<Guid?>("ModifiedById");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<Guid?>("ProjectId");

                    b.Property<Guid?>("WorkInProjectId");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ProjectId");

                    b.HasIndex("WorkInProjectId");

                    b.ToTable("ActivityAdditionalInfos");
                });

            modelBuilder.Entity("AcMan.Server.Models.EndSystem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid?>("ModifiedById");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("EndSystem");
                });

            modelBuilder.Entity("AcMan.Server.Models.Project", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid?>("EndSystemRecordId");

                    b.Property<bool>("IsSynchronized");

                    b.Property<Guid?>("ModifiedById");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("AcMan.Server.Models.Synchronization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int>("Duration");

                    b.Property<DateTime>("EndPeriod");

                    b.Property<Guid?>("EndSystemId");

                    b.Property<string>("Info");

                    b.Property<Guid?>("ModifiedById");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<DateTime>("StartPeriod");

                    b.HasKey("Id");

                    b.HasIndex("EndSystemId");

                    b.ToTable("Synchronization");
                });

            modelBuilder.Entity("AcMan.Server.Models.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ActivityAdditionalInfoId");

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid?>("ModifiedById");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("ActivityAdditionalInfoId");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("AcMan.Server.Models.TagInActivity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ActivityId");

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid?>("ModifiedById");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<Guid>("TagId");

                    b.HasKey("Id");

                    b.HasIndex("ActivityId");

                    b.HasIndex("TagId");

                    b.ToTable("TagInActivity");
                });

            modelBuilder.Entity("AcMan.Server.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid?>("EndSystemRecordId");

                    b.Property<bool>("IsSynchronized");

                    b.Property<DateTime?>("LastSyncDate");

                    b.Property<string>("Login");

                    b.Property<Guid?>("ModifiedById");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("AcMan.Server.Models.UserInSystem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid?>("EndSystemId");

                    b.Property<Guid?>("EndSystemRecordId");

                    b.Property<bool>("IsSynchronized");

                    b.Property<string>("Key")
                        .IsRequired();

                    b.Property<Guid?>("ModifiedById");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("EndSystemId");

                    b.HasIndex("UserId");

                    b.ToTable("UserInSystem");
                });

            modelBuilder.Entity("AcMan.Server.Models.WorkInProject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CreatedById");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<Guid?>("EndSystemRecordId");

                    b.Property<bool>("IsSynchronized");

                    b.Property<Guid?>("ModifiedById");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("Name");

                    b.Property<Guid?>("ProjectId");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("WorkInProject");
                });

            modelBuilder.Entity("AcMan.Server.Models.Activity", b =>
                {
                    b.HasOne("AcMan.Server.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId");

                    b.HasOne("AcMan.Server.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");

                    b.HasOne("AcMan.Server.Models.User", "User")
                        .WithMany("Activities")
                        .HasForeignKey("UserId");

                    b.HasOne("AcMan.Server.Models.WorkInProject", "WorkInProject")
                        .WithMany()
                        .HasForeignKey("WorkInProjectId");
                });

            modelBuilder.Entity("AcMan.Server.Models.ActivityAdditionalInfo", b =>
                {
                    b.HasOne("AcMan.Server.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId");

                    b.HasOne("AcMan.Server.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");

                    b.HasOne("AcMan.Server.Models.WorkInProject", "WorkInProject")
                        .WithMany()
                        .HasForeignKey("WorkInProjectId");
                });

            modelBuilder.Entity("AcMan.Server.Models.Synchronization", b =>
                {
                    b.HasOne("AcMan.Server.Models.EndSystem", "EndSystem")
                        .WithMany()
                        .HasForeignKey("EndSystemId");
                });

            modelBuilder.Entity("AcMan.Server.Models.Tag", b =>
                {
                    b.HasOne("AcMan.Server.Models.ActivityAdditionalInfo", "ActivityAdditionalInfo")
                        .WithMany("Tags")
                        .HasForeignKey("ActivityAdditionalInfoId");
                });

            modelBuilder.Entity("AcMan.Server.Models.TagInActivity", b =>
                {
                    b.HasOne("AcMan.Server.Models.Activity", "Activity")
                        .WithMany("TagInActivities")
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AcMan.Server.Models.Tag", "Tag")
                        .WithMany("TagInActivities")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AcMan.Server.Models.UserInSystem", b =>
                {
                    b.HasOne("AcMan.Server.Models.EndSystem", "EndSystem")
                        .WithMany()
                        .HasForeignKey("EndSystemId");

                    b.HasOne("AcMan.Server.Models.User", "User")
                        .WithMany("UserInSystems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AcMan.Server.Models.WorkInProject", b =>
                {
                    b.HasOne("AcMan.Server.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId");
                });
#pragma warning restore 612, 618
        }
    }
}
