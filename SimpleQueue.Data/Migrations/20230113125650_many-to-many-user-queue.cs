using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleQueue.Data.Migrations
{
    public partial class manytomanyuserqueue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserInQueues",
                columns: table => new
                {
                    UserInQueueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    QueueId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    JoinTime = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    NextId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInQueues", x => x.UserInQueueId);
                    table.ForeignKey(
                        name: "FK_UserInQueues_Queues_QueueId",
                        column: x => x.QueueId,
                        principalTable: "Queues",
                        principalColumn: "QueueId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInQueues_UserInQueues_NextId",
                        column: x => x.NextId,
                        principalTable: "UserInQueues",
                        principalColumn: "UserInQueueId");
                    table.ForeignKey(
                        name: "FK_UserInQueues_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_UserInQueues_NextId",
                table: "UserInQueues",
                column: "NextId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInQueues_QueueId",
                table: "UserInQueues",
                column: "QueueId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInQueues_UserId",
                table: "UserInQueues",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserInQueues");
        }
    }
}
