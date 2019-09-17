using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AcMan.Server.Migrations
{
    public partial class _20180703 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    EndSystemRecordId = table.Column<Guid>(nullable: true),
                    IsSynchronized = table.Column<bool>(nullable: false),
                    ModifiedById = table.Column<Guid>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EndSystem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedById = table.Column<Guid>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndSystem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    EndSystemRecordId = table.Column<Guid>(nullable: true),
                    IsSynchronized = table.Column<bool>(nullable: false),
                    ModifiedById = table.Column<Guid>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    EndSystemRecordId = table.Column<Guid>(nullable: true),
                    IsSynchronized = table.Column<bool>(nullable: false),
                    LastSyncDate = table.Column<DateTime>(nullable: true),
                    Login = table.Column<string>(nullable: true),
                    ModifiedById = table.Column<Guid>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Synchronization",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    EndPeriod = table.Column<DateTime>(nullable: false),
                    EndSystemId = table.Column<Guid>(nullable: true),
                    Info = table.Column<string>(nullable: true),
                    ModifiedById = table.Column<Guid>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    StartPeriod = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Synchronization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Synchronization_EndSystem_EndSystemId",
                        column: x => x.EndSystemId,
                        principalTable: "EndSystem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkInProject",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    EndSystemRecordId = table.Column<Guid>(nullable: true),
                    IsSynchronized = table.Column<bool>(nullable: false),
                    ModifiedById = table.Column<Guid>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ProjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkInProject", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkInProject_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserInSystem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    EndSystemId = table.Column<Guid>(nullable: true),
                    EndSystemRecordId = table.Column<Guid>(nullable: true),
                    IsSynchronized = table.Column<bool>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    ModifiedById = table.Column<Guid>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInSystem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserInSystem_EndSystem_EndSystemId",
                        column: x => x.EndSystemId,
                        principalTable: "EndSystem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserInSystem_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: true),
                    Caption = table.Column<string>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    End = table.Column<DateTime>(nullable: true),
                    EndSystemRecordId = table.Column<Guid>(nullable: true),
                    IsSynchronized = table.Column<bool>(nullable: false),
                    JiraUrl = table.Column<string>(nullable: true),
                    ModifiedById = table.Column<Guid>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: true),
                    Start = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: true),
                    WorkInProjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activity_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activity_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Activity_WorkInProject_WorkInProjectId",
                        column: x => x.WorkInProjectId,
                        principalTable: "WorkInProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ActivityAdditionalInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    JiraUrl = table.Column<string>(nullable: true),
                    ModifiedById = table.Column<Guid>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: true),
                    WorkInProjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityAdditionalInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityAdditionalInfos_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityAdditionalInfos_Project_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActivityAdditionalInfos_WorkInProject_WorkInProjectId",
                        column: x => x.WorkInProjectId,
                        principalTable: "WorkInProject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ActivityAdditionalInfoId = table.Column<Guid>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedById = table.Column<Guid>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_ActivityAdditionalInfos_ActivityAdditionalInfoId",
                        column: x => x.ActivityAdditionalInfoId,
                        principalTable: "ActivityAdditionalInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TagInActivity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ActivityId = table.Column<Guid>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedById = table.Column<Guid>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    TagId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagInActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagInActivity_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagInActivity_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_AccountId",
                table: "Activity",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ProjectId",
                table: "Activity",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_UserId",
                table: "Activity",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_WorkInProjectId",
                table: "Activity",
                column: "WorkInProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAdditionalInfos_AccountId",
                table: "ActivityAdditionalInfos",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAdditionalInfos_ProjectId",
                table: "ActivityAdditionalInfos",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAdditionalInfos_WorkInProjectId",
                table: "ActivityAdditionalInfos",
                column: "WorkInProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Synchronization_EndSystemId",
                table: "Synchronization",
                column: "EndSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ActivityAdditionalInfoId",
                table: "Tag",
                column: "ActivityAdditionalInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_TagInActivity_ActivityId",
                table: "TagInActivity",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_TagInActivity_TagId",
                table: "TagInActivity",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInSystem_EndSystemId",
                table: "UserInSystem",
                column: "EndSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInSystem_UserId",
                table: "UserInSystem",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkInProject_ProjectId",
                table: "WorkInProject",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Synchronization");

            migrationBuilder.DropTable(
                name: "TagInActivity");

            migrationBuilder.DropTable(
                name: "UserInSystem");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "EndSystem");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "ActivityAdditionalInfos");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "WorkInProject");

            migrationBuilder.DropTable(
                name: "Project");
        }
    }
}
