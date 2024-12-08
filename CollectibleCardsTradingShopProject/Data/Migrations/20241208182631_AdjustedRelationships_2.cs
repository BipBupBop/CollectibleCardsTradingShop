using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollectibleCardsTradingShopProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedRelationships_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lots_AspNetUsers_ClosedUserId",
                schema: "identity",
                table: "Lots");

            migrationBuilder.DropForeignKey(
                name: "FK_Lots_AspNetUsers_UserId",
                schema: "identity",
                table: "Lots");

            migrationBuilder.DropIndex(
                name: "IX_Lots_ClosedUserId",
                schema: "identity",
                table: "Lots");

            migrationBuilder.DropIndex(
                name: "IX_Lots_UserId",
                schema: "identity",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "ClosedUserId",
                schema: "identity",
                table: "Lots");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "identity",
                table: "Lots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClosedUserId",
                schema: "identity",
                table: "Lots",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "identity",
                table: "Lots",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_ClosedUserId",
                schema: "identity",
                table: "Lots",
                column: "ClosedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Lots_UserId",
                schema: "identity",
                table: "Lots",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_AspNetUsers_ClosedUserId",
                schema: "identity",
                table: "Lots",
                column: "ClosedUserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_AspNetUsers_UserId",
                schema: "identity",
                table: "Lots",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
