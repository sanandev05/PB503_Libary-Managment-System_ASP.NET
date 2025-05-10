using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PB503_Libary_Managment_System_ASP.NET.Migrations
{
    /// <inheritdoc />
    public partial class AddedImageURL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageURL",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageURL",
                table: "Books");
        }
    }
}
