using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollectibleCardsTradingShopProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedRelationships : Migration
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

            migrationBuilder.CreateTable(
                name: "UserLot",
                schema: "identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DidCloseTheLot = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LotId = table.Column<int>(type: "int", nullable: false),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLot_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalSchema: "identity",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserLot_Lots_LotId",
                        column: x => x.LotId,
                        principalSchema: "identity",
                        principalTable: "Lots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLot_LotId",
                schema: "identity",
                table: "UserLot",
                column: "LotId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLot_UserId1",
                schema: "identity",
                table: "UserLot",
                column: "UserId1");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lots_AspNetUsers_ClosedUserId",
                schema: "identity",
                table: "Lots");

            migrationBuilder.DropForeignKey(
                name: "FK_Lots_AspNetUsers_UserId",
                schema: "identity",
                table: "Lots");

            migrationBuilder.DropTable(
                name: "UserLot",
                schema: "identity");

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_AspNetUsers_ClosedUserId",
                schema: "identity",
                table: "Lots",
                column: "ClosedUserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Lots_AspNetUsers_UserId",
                schema: "identity",
                table: "Lots",
                column: "UserId",
                principalSchema: "identity",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
