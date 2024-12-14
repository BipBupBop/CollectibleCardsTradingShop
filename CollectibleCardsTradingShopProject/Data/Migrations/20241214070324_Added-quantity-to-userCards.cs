using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollectibleCardsTradingShopProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedquantitytouserCards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                schema: "identity",
                table: "UserCards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "identity",
                table: "UserCards");
        }
    }
}
