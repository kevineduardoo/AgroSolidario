using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroSolidario.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusAlimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Alimentos",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Alimentos");
        }
    }
}
