using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleQueue.Data.Migrations
{
    public partial class isFrozennamingfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isFrozen",
                table: "Queues",
                newName: "IsFrozen");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsFrozen",
                table: "Queues",
                newName: "isFrozen");
        }
    }
}
