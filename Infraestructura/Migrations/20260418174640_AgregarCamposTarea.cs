using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructura.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposTarea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Recursos",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Estado",
                table: "Recursos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Recursos",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaVencimiento",
                table: "Recursos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Prioridad",
                table: "Recursos",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_Estado",
                table: "Recursos",
                column: "Estado");

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_FechaVencimiento",
                table: "Recursos",
                column: "FechaVencimiento");

            migrationBuilder.CreateIndex(
                name: "IX_Recursos_Prioridad",
                table: "Recursos",
                column: "Prioridad");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Recursos_Estado",
                table: "Recursos");

            migrationBuilder.DropIndex(
                name: "IX_Recursos_FechaVencimiento",
                table: "Recursos");

            migrationBuilder.DropIndex(
                name: "IX_Recursos_Prioridad",
                table: "Recursos");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Recursos");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Recursos");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Recursos");

            migrationBuilder.DropColumn(
                name: "FechaVencimiento",
                table: "Recursos");

            migrationBuilder.DropColumn(
                name: "Prioridad",
                table: "Recursos");
        }
    }
}
