using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroSolidario.Migrations
{
    /// <inheritdoc />
    public partial class AddFotoAlimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Foto",
                table: "Alimentos",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foto",
                table: "Alimentos");
        }
    }
}
