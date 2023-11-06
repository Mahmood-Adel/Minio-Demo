using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Minio_Demo.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "library_files",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name_ar = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    name_en = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    file_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    file_size = table.Column<long>(type: "bigint", nullable: true),
                    content_type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_library_files", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "library_files");
        }
    }
}
