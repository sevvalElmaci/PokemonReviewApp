using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    public partial class AddUserRolePermissionTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedDateTime", "CreatedUserId", "DeletedDateTime", "DeletedUserId", "IsDeleted", "Name", "UpdatedDateTime", "UpdatedUserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 12, 6, 43, 32, 700, DateTimeKind.Utc).AddTicks(1898), null, null, null, false, "ListPokemon", null, null },
                    { 2, new DateTime(2025, 11, 12, 6, 43, 32, 700, DateTimeKind.Utc).AddTicks(1899), null, null, null, false, "AddPokemon", null, null },
                    { 3, new DateTime(2025, 11, 12, 6, 43, 32, 700, DateTimeKind.Utc).AddTicks(1900), null, null, null, false, "UpdatePokemon", null, null },
                    { 4, new DateTime(2025, 11, 12, 6, 43, 32, 700, DateTimeKind.Utc).AddTicks(1900), null, null, null, false, "DeletePokemon", null, null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedDateTime", "CreatedUserId", "DeletedDateTime", "DeletedUserId", "IsDeleted", "Name", "UpdatedDateTime", "UpdatedUserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 12, 6, 43, 32, 700, DateTimeKind.Utc).AddTicks(1753), null, null, null, false, "Admin", null, null },
                    { 2, new DateTime(2025, 11, 12, 6, 43, 32, 700, DateTimeKind.Utc).AddTicks(1783), null, null, null, false, "Manager", null, null },
                    { 3, new DateTime(2025, 11, 12, 6, 43, 32, 700, DateTimeKind.Utc).AddTicks(1784), null, null, null, false, "User", null, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
