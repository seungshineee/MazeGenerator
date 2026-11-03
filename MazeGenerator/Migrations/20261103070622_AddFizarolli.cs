using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MazeGenerator.Migrations
{
    /// <inheritdoc />
    public partial class AddFizarolli : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Character",
                columns: new[] { "Id", "ImagePath", "Name" },
                values: new object[] { 4, "/images/fizarolli.png", "Физаролли" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Character",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
