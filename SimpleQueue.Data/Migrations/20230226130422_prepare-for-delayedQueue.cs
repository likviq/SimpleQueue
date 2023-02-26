using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleQueue.Data.Migrations
{
    public partial class preparefordelayedQueue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DestinationTime",
                table: "UserInQueues",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "QueueTypeId",
                table: "Queues",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "QueueType",
                columns: table => new
                {
                    QueueTypeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueType", x => x.QueueTypeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Queues_QueueTypeId",
                table: "Queues",
                column: "QueueTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Queues_QueueType_QueueTypeId",
                table: "Queues",
                column: "QueueTypeId",
                principalTable: "QueueType",
                principalColumn: "QueueTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Queues_QueueType_QueueTypeId",
                table: "Queues");

            migrationBuilder.DropTable(
                name: "QueueType");

            migrationBuilder.DropIndex(
                name: "IX_Queues_QueueTypeId",
                table: "Queues");

            migrationBuilder.DropColumn(
                name: "DestinationTime",
                table: "UserInQueues");

            migrationBuilder.DropColumn(
                name: "QueueTypeId",
                table: "Queues");
        }
    }
}
