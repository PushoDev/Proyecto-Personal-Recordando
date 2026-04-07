using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recursos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    UmbralMinimo = table.Column<int>(type: "int", nullable: false),
                    UrlOriginal = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CodigoCorto = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Clicks = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recursos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClickLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecursoId = table.Column<int>(type: "int", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IpOrigen = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClickLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClickLogs_Recursos_RecursoId",
                        column: x => x.RecursoId,
                        principalTable: "Recursos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClickLogs_FechaHora",
                table: "ClickLogs",
                column: "FechaHora");

            migrationBuilder.CreateIndex(
                name: "IX_ClickLogs_RecursoId",
                table: "ClickLogs",
                column: "RecursoId");

            migrationBuilder.CreateIndex(
                name: "IX_ClickLogs_RecursoId_FechaHora",
                table: "ClickLogs",
                columns: new[] { "RecursoId", "FechaHora" });

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_CodigoCorto",
                table: "Recursos",
                column: "CodigoCorto",
                unique: true,
                filter: "[CodigoCorto] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_Nombre",
                table: "Recursos",
                column: "Nombre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClickLogs");

            migrationBuilder.DropTable(
                name: "Recursos");
        }
    }
}
