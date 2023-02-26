using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleQueue.Data.Migrations
{
    public partial class userInQueueuseridnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInQueues_Users_UserId",
                table: "UserInQueues");

            migrationBuilder.DeleteData(
                table: "QueueType",
                keyColumn: "QueueTypeId",
                keyValue: new Guid("640274a8-159b-43ff-9cb4-ad83191af2d1"));

            migrationBuilder.DeleteData(
                table: "QueueType",
                keyColumn: "QueueTypeId",
                keyValue: new Guid("b62c54da-5e3a-4bc2-b05c-9494ffe8ece8"));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserInQueues",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DestinationTime",
                table: "UserInQueues",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.InsertData(
                table: "QueueType",
                columns: new[] { "QueueTypeId", "Name" },
                values: new object[] { new Guid("2550a9b8-55c5-499d-82db-0f151ff291c5"), 0 });

            migrationBuilder.InsertData(
                table: "QueueType",
                columns: new[] { "QueueTypeId", "Name" },
                values: new object[] { new Guid("4a0ede84-0d59-4a97-9a82-96d8e386c730"), 1 });

            migrationBuilder.AddForeignKey(
                name: "FK_UserInQueues_Users_UserId",
                table: "UserInQueues",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserInQueues_Users_UserId",
                table: "UserInQueues");

            migrationBuilder.DeleteData(
                table: "QueueType",
                keyColumn: "QueueTypeId",
                keyValue: new Guid("2550a9b8-55c5-499d-82db-0f151ff291c5"));

            migrationBuilder.DeleteData(
                table: "QueueType",
                keyColumn: "QueueTypeId",
                keyValue: new Guid("4a0ede84-0d59-4a97-9a82-96d8e386c730"));

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserInQueues",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DestinationTime",
                table: "UserInQueues",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "QueueType",
                columns: new[] { "QueueTypeId", "Name" },
                values: new object[] { new Guid("640274a8-159b-43ff-9cb4-ad83191af2d1"), 1 });

            migrationBuilder.InsertData(
                table: "QueueType",
                columns: new[] { "QueueTypeId", "Name" },
                values: new object[] { new Guid("b62c54da-5e3a-4bc2-b05c-9494ffe8ece8"), 0 });

            migrationBuilder.AddForeignKey(
                name: "FK_UserInQueues_Users_UserId",
                table: "UserInQueues",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
