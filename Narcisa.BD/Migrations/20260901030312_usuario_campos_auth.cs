using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Narcisa.BD.Migrations
{
    /// <inheritdoc />
    public partial class usuario_campos_auth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "createdAt",
                table: "Usuario",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "rol",
                table: "Usuario",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "createdAt",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "email",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "password",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "rol",
                table: "Usuario");
        }
    }
}
