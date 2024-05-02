using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MGOBankApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddingCVV : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardCVV",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardCVV",
                table: "Cards");
        }
    }
}
