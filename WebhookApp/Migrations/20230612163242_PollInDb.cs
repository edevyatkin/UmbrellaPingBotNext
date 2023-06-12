using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebhookApp.Migrations
{
    public partial class PollInDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BattlePolls",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TgChatId = table.Column<long>(type: "INTEGER", nullable: false),
                    TgMessageId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattlePolls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BattlePin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TgChatId = table.Column<long>(type: "INTEGER", nullable: false),
                    TgMessageId = table.Column<long>(type: "INTEGER", nullable: false),
                    BattlePollId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattlePin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattlePin_BattlePolls_BattlePollId",
                        column: x => x.BattlePollId,
                        principalTable: "BattlePolls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BattlePollOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BattlePollId = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderInOptions = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderInVotes = table.Column<int>(type: "INTEGER", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattlePollOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattlePollOption_BattlePolls_BattlePollId",
                        column: x => x.BattlePollId,
                        principalTable: "BattlePolls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BattlePollVote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BattlePollId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateTime = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattlePollVote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattlePollVote_BattlePolls_BattlePollId",
                        column: x => x.BattlePollId,
                        principalTable: "BattlePolls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BattlePin_BattlePollId",
                table: "BattlePin",
                column: "BattlePollId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BattlePollOption_BattlePollId",
                table: "BattlePollOption",
                column: "BattlePollId");

            migrationBuilder.CreateIndex(
                name: "IX_BattlePollVote_BattlePollId",
                table: "BattlePollVote",
                column: "BattlePollId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BattlePin");

            migrationBuilder.DropTable(
                name: "BattlePollOption");

            migrationBuilder.DropTable(
                name: "BattlePollVote");

            migrationBuilder.DropTable(
                name: "BattlePolls");
        }
    }
}
