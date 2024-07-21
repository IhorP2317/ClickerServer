using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clicker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChannelSubscriptionTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChannelId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelSubscriptionTasks", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OfferSubscriptionTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OfferUrl = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferSubscriptionTasks", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TelegramId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Energy = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    ReferrerId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_ReferrerId",
                        column: x => x.ReferrerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserChannelSubscriptionTasks",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChannelSubscriptionTaskId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsCompleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChannelSubscriptionTasks", x => new { x.UserId, x.ChannelSubscriptionTaskId });
                    table.ForeignKey(
                        name: "FK_UserChannelSubscriptionTasks_ChannelSubscriptionTasks_Channe~",
                        column: x => x.ChannelSubscriptionTaskId,
                        principalTable: "ChannelSubscriptionTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChannelSubscriptionTasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserOfferSubscriptionTasks",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OfferSubscriptionTaskId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IsCompleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOfferSubscriptionTasks", x => new { x.UserId, x.OfferSubscriptionTaskId });
                    table.ForeignKey(
                        name: "FK_UserOfferSubscriptionTasks_OfferSubscriptionTasks_OfferSubsc~",
                        column: x => x.OfferSubscriptionTaskId,
                        principalTable: "OfferSubscriptionTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOfferSubscriptionTasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserChannelSubscriptionTasks_ChannelSubscriptionTaskId",
                table: "UserChannelSubscriptionTasks",
                column: "ChannelSubscriptionTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOfferSubscriptionTasks_OfferSubscriptionTaskId",
                table: "UserOfferSubscriptionTasks",
                column: "OfferSubscriptionTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReferrerId",
                table: "Users",
                column: "ReferrerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserChannelSubscriptionTasks");

            migrationBuilder.DropTable(
                name: "UserOfferSubscriptionTasks");

            migrationBuilder.DropTable(
                name: "ChannelSubscriptionTasks");

            migrationBuilder.DropTable(
                name: "OfferSubscriptionTasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
