using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollectibleCardsTradingShopProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class Connectedcardsandusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLot_AspNetUsers_UserId",
                schema: "identity",
                table: "UserLot");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLot_Lots_LotId",
                schema: "identity",
                table: "UserLot");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLot",
                schema: "identity",
                table: "UserLot");

            migrationBuilder.RenameTable(
                name: "UserLot",
                schema: "identity",
                newName: "UserLots",
                newSchema: "identity");

            migrationBuilder.RenameIndex(
                name: "IX_UserLot_UserId",
                schema: "identity",
                table: "UserLots",
                newName: "IX_UserLots_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLot_LotId",
                schema: "identity",
                table: "UserLots",
                newName: "IX_UserLots_LotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLots",
                schema: "identity",
                table: "UserLots",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserCards",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CardId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCards_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCards_Cards_CardId",
                        column: x => x.CardId,
                        principalSchema: "identity",
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCards_CardId",
                schema: "identity",
                table: "UserCards",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCards_UserId",
                schema: "identity",
                table: "UserCards",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLots_AspNetUsers_UserId",
                schema: "identity",
                table: "UserLots",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLots_Lots_LotId",
                schema: "identity",
                table: "UserLots",
                column: "LotId",
                principalSchema: "identity",
                principalTable: "Lots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLots_AspNetUsers_UserId",
                schema: "identity",
                table: "UserLots");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLots_Lots_LotId",
                schema: "identity",
                table: "UserLots");

            migrationBuilder.DropTable(
                name: "UserCards",
                schema: "identity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLots",
                schema: "identity",
                table: "UserLots");

            migrationBuilder.RenameTable(
                name: "UserLots",
                schema: "identity",
                newName: "UserLot",
                newSchema: "identity");

            migrationBuilder.RenameIndex(
                name: "IX_UserLots_UserId",
                schema: "identity",
                table: "UserLot",
                newName: "IX_UserLot_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLots_LotId",
                schema: "identity",
                table: "UserLot",
                newName: "IX_UserLot_LotId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLot",
                schema: "identity",
                table: "UserLot",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLot_AspNetUsers_UserId",
                schema: "identity",
                table: "UserLot",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLot_Lots_LotId",
                schema: "identity",
                table: "UserLot",
                column: "LotId",
                principalSchema: "identity",
                principalTable: "Lots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
