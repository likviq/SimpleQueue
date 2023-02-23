using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleQueue.Data.Migrations
{
    public partial class userinqueuepreviousparticipant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PreviousId",
                table: "UserInQueues",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_UserInQueues_PreviousId",
                table: "UserInQueues",
                column: "PreviousId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInQueues_UserInQueues_PreviousId",
                table: "UserInQueues",
                column: "PreviousId",
                principalTable: "UserInQueues",
                principalColumn: "UserInQueueId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInQueues_UserInQueues_PreviousId",
                table: "UserInQueues");

            migrationBuilder.DropIndex(
                name: "IX_UserInQueues_PreviousId",
                table: "UserInQueues");

            migrationBuilder.DropColumn(
                name: "PreviousId",
                table: "UserInQueues");
        }
    }
}
