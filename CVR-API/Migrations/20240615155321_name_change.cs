using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CVR_API.Migrations
{
    /// <inheritdoc />
    public partial class name_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CVR",
                table: "User",
                newName: "CPR");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CPR",
                table: "User",
                newName: "CVR");
        }
    }
}
