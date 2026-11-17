using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Narcisa.BD.Migrations
{
    /// <inheritdoc />
    public partial class Add_Producto_ImagenPngUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagenPngUrl",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenPngUrl",
                table: "Productos");
        }
    }
}
