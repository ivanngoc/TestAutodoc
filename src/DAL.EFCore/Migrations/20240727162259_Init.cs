using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    SomeTaskId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTimeCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.SomeTaskId);
                });

            migrationBuilder.CreateTable(
                name: "Metas",
                columns: table => new
                {
                    MetaId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileDir = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileNameOriginal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileExt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SomeTaskId = table.Column<long>(type: "bigint", nullable: false),
                    LengthTotal = table.Column<long>(type: "bigint", nullable: false),
                    LengthUploaded = table.Column<long>(type: "bigint", nullable: false),
                    IsUploaded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metas", x => x.MetaId);
                    table.ForeignKey(
                        name: "FK_Metas_Tasks_SomeTaskId",
                        column: x => x.SomeTaskId,
                        principalTable: "Tasks",
                        principalColumn: "SomeTaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Metas_ResourceId",
                table: "Metas",
                column: "ResourceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Metas_SomeTaskId",
                table: "Metas",
                column: "SomeTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Metas");

            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
