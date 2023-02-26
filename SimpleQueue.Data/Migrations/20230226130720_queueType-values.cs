using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleQueue.Data.Migrations
{
    public partial class queueTypevalues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "QueueType",
                columns: new[] { "QueueTypeId", "Name" },
                values: new object[] { new Guid("640274a8-159b-43ff-9cb4-ad83191af2d1"), 1 });

            migrationBuilder.InsertData(
                table: "QueueType",
                columns: new[] { "QueueTypeId", "Name" },
                values: new object[] { new Guid("b62c54da-5e3a-4bc2-b05c-9494ffe8ece8"), 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "QueueType",
                keyColumn: "QueueTypeId",
                keyValue: new Guid("640274a8-159b-43ff-9cb4-ad83191af2d1"));

            migrationBuilder.DeleteData(
                table: "QueueType",
                keyColumn: "QueueTypeId",
                keyValue: new Guid("b62c54da-5e3a-4bc2-b05c-9494ffe8ece8"));
        }
    }
}
