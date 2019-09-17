using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AcMan.Server.Migrations
{
    public partial class AcManServerCoreDBAcManContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EndSystem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndSystem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TagStrategy",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagStrategy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Login = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Caption = table.Column<string>(nullable: true),
                    ContactId = table.Column<Guid>(nullable: true),
                    End = table.Column<DateTime>(nullable: true),
                    Start = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactInSystem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContactId = table.Column<Guid>(nullable: true),
                    EndSystemId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInSystem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactInSystem_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contact",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactInSystem_EndSystem_EndSystemId",
                        column: x => x.EndSystemId,
                        principalTable: "EndSystem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TagType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TagStrategyId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TagType_TagStrategy_TagStrategyId",
                        column: x => x.TagStrategyId,
                        principalTable: "TagStrategy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ActivityId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    TypeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tag_TagStrategy_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TagStrategy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ContactId",
                table: "Activity",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInSystem_ContactId",
                table: "ContactInSystem",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInSystem_EndSystemId",
                table: "ContactInSystem",
                column: "EndSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_ActivityId",
                table: "Tag",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_TypeId",
                table: "Tag",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TagType_TagStrategyId",
                table: "TagType",
                column: "TagStrategyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactInSystem");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "TagType");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "EndSystem");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "TagStrategy");

            migrationBuilder.DropTable(
                name: "Contact");
        }
    }
}
