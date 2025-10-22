using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    public partial class AddQuantityProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PokeFoods_Quantities_QuantityId",
                table: "PokeFoods");

            migrationBuilder.DropForeignKey(
                name: "FK_PokeFoods_Quantities_QuantityId1",
                table: "PokeFoods");

            migrationBuilder.DropTable(
                name: "Quantities");

            migrationBuilder.DropIndex(
                name: "IX_PokeFoods_QuantityId",
                table: "PokeFoods");

            migrationBuilder.DropIndex(
                name: "IX_PokeFoods_QuantityId1",
                table: "PokeFoods");

            migrationBuilder.DropColumn(
                name: "QuantityId",
                table: "PokeFoods");

            migrationBuilder.DropColumn(
                name: "QuantityId1",
                table: "PokeFoods");

            migrationBuilder.AddColumn<double>(
                name: "Quantity",
                table: "PokeFoods",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PokeFoods");

            migrationBuilder.AddColumn<int>(
                name: "QuantityId",
                table: "PokeFoods",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuantityId1",
                table: "PokeFoods",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Quantities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quantities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PokeFoods_QuantityId",
                table: "PokeFoods",
                column: "QuantityId");

            migrationBuilder.CreateIndex(
                name: "IX_PokeFoods_QuantityId1",
                table: "PokeFoods",
                column: "QuantityId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PokeFoods_Quantities_QuantityId",
                table: "PokeFoods",
                column: "QuantityId",
                principalTable: "Quantities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PokeFoods_Quantities_QuantityId1",
                table: "PokeFoods",
                column: "QuantityId1",
                principalTable: "Quantities",
                principalColumn: "Id");
        }
    }
}
