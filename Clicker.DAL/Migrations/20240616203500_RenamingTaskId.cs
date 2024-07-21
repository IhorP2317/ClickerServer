using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clicker.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenamingTaskId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChannelSubscriptionTasks_ChannelSubscriptionTasks_Channe~",
                table: "UserChannelSubscriptionTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOfferSubscriptionTasks_OfferSubscriptionTasks_OfferSubsc~",
                table: "UserOfferSubscriptionTasks");

            migrationBuilder.RenameColumn(
                name: "OfferSubscriptionTaskId",
                table: "UserOfferSubscriptionTasks",
                newName: "TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_UserOfferSubscriptionTasks_OfferSubscriptionTaskId",
                table: "UserOfferSubscriptionTasks",
                newName: "IX_UserOfferSubscriptionTasks_TaskId");

            migrationBuilder.RenameColumn(
                name: "ChannelSubscriptionTaskId",
                table: "UserChannelSubscriptionTasks",
                newName: "TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_UserChannelSubscriptionTasks_ChannelSubscriptionTaskId",
                table: "UserChannelSubscriptionTasks",
                newName: "IX_UserChannelSubscriptionTasks_TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserChannelSubscriptionTasks_ChannelSubscriptionTasks_TaskId",
                table: "UserChannelSubscriptionTasks",
                column: "TaskId",
                principalTable: "ChannelSubscriptionTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOfferSubscriptionTasks_OfferSubscriptionTasks_TaskId",
                table: "UserOfferSubscriptionTasks",
                column: "TaskId",
                principalTable: "OfferSubscriptionTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChannelSubscriptionTasks_ChannelSubscriptionTasks_TaskId",
                table: "UserChannelSubscriptionTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOfferSubscriptionTasks_OfferSubscriptionTasks_TaskId",
                table: "UserOfferSubscriptionTasks");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "UserOfferSubscriptionTasks",
                newName: "OfferSubscriptionTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_UserOfferSubscriptionTasks_TaskId",
                table: "UserOfferSubscriptionTasks",
                newName: "IX_UserOfferSubscriptionTasks_OfferSubscriptionTaskId");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "UserChannelSubscriptionTasks",
                newName: "ChannelSubscriptionTaskId");

            migrationBuilder.RenameIndex(
                name: "IX_UserChannelSubscriptionTasks_TaskId",
                table: "UserChannelSubscriptionTasks",
                newName: "IX_UserChannelSubscriptionTasks_ChannelSubscriptionTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserChannelSubscriptionTasks_ChannelSubscriptionTasks_Channe~",
                table: "UserChannelSubscriptionTasks",
                column: "ChannelSubscriptionTaskId",
                principalTable: "ChannelSubscriptionTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOfferSubscriptionTasks_OfferSubscriptionTasks_OfferSubsc~",
                table: "UserOfferSubscriptionTasks",
                column: "OfferSubscriptionTaskId",
                principalTable: "OfferSubscriptionTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
