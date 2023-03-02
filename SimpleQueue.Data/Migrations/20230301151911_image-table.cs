using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleQueue.Data.Migrations
{
    public partial class imagetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ImageBlobId",
                table: "Queues",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateTable(
                name: "ImageBlob",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImageLink = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageBlob", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Queues_ImageBlobId",
                table: "Queues",
                column: "ImageBlobId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Queues_ImageBlob_ImageBlobId",
                table: "Queues",
                column: "ImageBlobId",
                principalTable: "ImageBlob",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Queues_ImageBlob_ImageBlobId",
                table: "Queues");

            migrationBuilder.DropTable(
                name: "ImageBlob");

            migrationBuilder.DropIndex(
                name: "IX_Queues_ImageBlobId",
                table: "Queues");

            migrationBuilder.DropColumn(
                name: "ImageBlobId",
                table: "Queues");
        }
    }
}
