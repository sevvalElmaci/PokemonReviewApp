using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    public partial class AddAuditColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "RolePermissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserId",
                table: "RolePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDateTime",
                table: "RolePermissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "RolePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RolePermissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "RolePermissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedUserId",
                table: "RolePermissions",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDateTime", "PermissionName" },
                values: new object[] { new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1274), "ListCategory" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDateTime", "PermissionName" },
                values: new object[] { new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1286), "AddCategory" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDateTime", "PermissionName" },
                values: new object[] { new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1287), "UpdateCategory" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDateTime", "PermissionName" },
                values: new object[] { new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1288), "DeleteCategory" });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CreatedDateTime", "CreatedUserId", "DeletedDateTime", "DeletedUserId", "Description", "IsDeleted", "PermissionName", "UpdatedDateTime", "UpdatedUserId" },
                values: new object[,]
                {
                    { 5, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1289), null, null, null, null, false, "ListCountry", null, null },
                    { 6, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1291), null, null, null, null, false, "AddCountry", null, null },
                    { 7, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1311), null, null, null, null, false, "UpdateCountry", null, null },
                    { 8, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1312), null, null, null, null, false, "DeleteCountry", null, null },
                    { 9, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1313), null, null, null, null, false, "ListFood", null, null },
                    { 10, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1314), null, null, null, null, false, "AddFood", null, null },
                    { 11, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1315), null, null, null, null, false, "UpdateFood", null, null },
                    { 12, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1316), null, null, null, null, false, "DeleteFood", null, null },
                    { 13, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1316), null, null, null, null, false, "ListOwner", null, null },
                    { 14, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1317), null, null, null, null, false, "AddOwner", null, null },
                    { 15, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1318), null, null, null, null, false, "UpdateOwner", null, null },
                    { 16, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1318), null, null, null, null, false, "DeleteOwner", null, null },
                    { 17, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1319), null, null, null, null, false, "ListPokemon", null, null },
                    { 18, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1320), null, null, null, null, false, "AddPokemon", null, null },
                    { 19, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1321), null, null, null, null, false, "UpdatePokemon", null, null },
                    { 20, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1322), null, null, null, null, false, "DeletePokemon", null, null },
                    { 21, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1322), null, null, null, null, false, "ListProperty", null, null },
                    { 22, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1323), null, null, null, null, false, "AddProperty", null, null },
                    { 23, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1324), null, null, null, null, false, "UpdateProperty", null, null },
                    { 24, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1324), null, null, null, null, false, "DeleteProperty", null, null },
                    { 25, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1325), null, null, null, null, false, "ListReview", null, null },
                    { 26, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1325), null, null, null, null, false, "AddReview", null, null },
                    { 27, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1326), null, null, null, null, false, "UpdateReview", null, null },
                    { 28, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1327), null, null, null, null, false, "DeleteReview", null, null },
                    { 29, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1327), null, null, null, null, false, "ListReviewer", null, null },
                    { 30, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1328), null, null, null, null, false, "AddReviewer", null, null },
                    { 31, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1328), null, null, null, null, false, "UpdateReviewer", null, null },
                    { 32, new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1329), null, null, null, null, false, "DeleteReviewer", null, null }
                });

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 1, 1 },
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1538));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 2, 1 },
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1540));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 3, 1 },
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1541));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 4, 1 },
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1541));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 1, 2 },
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1585));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 2, 2 },
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1588));

            migrationBuilder.UpdateData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 1, 3 },
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1759));

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId", "CreatedDateTime", "CreatedUserId", "DeletedDateTime", "DeletedUserId", "IsDeleted", "UpdatedDateTime", "UpdatedUserId" },
                values: new object[] { 3, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1590), null, null, null, false, null, null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1422));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1423));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 14, 11, 42, 59, 473, DateTimeKind.Local).AddTicks(1424));

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId", "CreatedDateTime", "CreatedUserId", "DeletedDateTime", "DeletedUserId", "IsDeleted", "UpdatedDateTime", "UpdatedUserId" },
                values: new object[,]
                {
                    { 5, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1541), null, null, null, false, null, null },
                    { 6, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1542), null, null, null, false, null, null },
                    { 7, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1543), null, null, null, false, null, null },
                    { 8, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1543), null, null, null, false, null, null },
                    { 9, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1544), null, null, null, false, null, null },
                    { 10, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1544), null, null, null, false, null, null },
                    { 11, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1545), null, null, null, false, null, null },
                    { 12, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1545), null, null, null, false, null, null },
                    { 13, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1545), null, null, null, false, null, null },
                    { 14, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1546), null, null, null, false, null, null },
                    { 15, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1546), null, null, null, false, null, null },
                    { 16, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1546), null, null, null, false, null, null },
                    { 17, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1547), null, null, null, false, null, null },
                    { 18, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1548), null, null, null, false, null, null },
                    { 19, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1548), null, null, null, false, null, null },
                    { 20, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1548), null, null, null, false, null, null },
                    { 21, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1549), null, null, null, false, null, null },
                    { 22, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1549), null, null, null, false, null, null },
                    { 23, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1549), null, null, null, false, null, null },
                    { 24, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1569), null, null, null, false, null, null },
                    { 25, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1569), null, null, null, false, null, null },
                    { 26, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1569), null, null, null, false, null, null },
                    { 27, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1570), null, null, null, false, null, null },
                    { 28, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1570), null, null, null, false, null, null },
                    { 29, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1571), null, null, null, false, null, null },
                    { 30, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1571), null, null, null, false, null, null },
                    { 31, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1571), null, null, null, false, null, null },
                    { 32, 1, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1571), null, null, null, false, null, null },
                    { 5, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1592), null, null, null, false, null, null },
                    { 6, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1593), null, null, null, false, null, null },
                    { 7, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1595), null, null, null, false, null, null },
                    { 9, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1596), null, null, null, false, null, null },
                    { 10, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1597), null, null, null, false, null, null },
                    { 11, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1598), null, null, null, false, null, null },
                    { 13, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1599), null, null, null, false, null, null },
                    { 14, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1600), null, null, null, false, null, null },
                    { 15, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1601), null, null, null, false, null, null },
                    { 17, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1603), null, null, null, false, null, null },
                    { 18, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1604), null, null, null, false, null, null },
                    { 19, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1605), null, null, null, false, null, null },
                    { 21, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1608), null, null, null, false, null, null },
                    { 22, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1609), null, null, null, false, null, null }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId", "CreatedDateTime", "CreatedUserId", "DeletedDateTime", "DeletedUserId", "IsDeleted", "UpdatedDateTime", "UpdatedUserId" },
                values: new object[,]
                {
                    { 23, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1610), null, null, null, false, null, null },
                    { 25, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1613), null, null, null, false, null, null },
                    { 26, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1614), null, null, null, false, null, null },
                    { 27, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1615), null, null, null, false, null, null },
                    { 29, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1617), null, null, null, false, null, null },
                    { 30, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1618), null, null, null, false, null, null },
                    { 31, 2, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1619), null, null, null, false, null, null },
                    { 5, 3, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1760), null, null, null, false, null, null },
                    { 9, 3, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1760), null, null, null, false, null, null },
                    { 13, 3, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1761), null, null, null, false, null, null },
                    { 17, 3, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1761), null, null, null, false, null, null },
                    { 21, 3, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1761), null, null, null, false, null, null },
                    { 25, 3, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1762), null, null, null, false, null, null },
                    { 26, 3, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1762), null, null, null, false, null, null },
                    { 29, 3, new DateTime(2025, 11, 14, 8, 42, 59, 473, DateTimeKind.Utc).AddTicks(1762), null, null, null, false, null, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 6, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 7, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 8, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 9, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 10, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 11, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 12, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 13, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 14, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 15, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 16, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 17, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 18, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 19, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 20, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 21, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 22, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 23, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 24, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 25, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 26, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 27, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 28, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 29, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 30, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 31, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 32, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 6, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 7, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 9, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 10, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 11, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 13, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 14, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 15, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 17, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 18, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 19, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 21, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 22, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 23, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 25, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 26, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 27, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 29, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 30, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 31, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 5, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 9, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 13, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 17, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 21, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 25, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 26, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 29, 3 });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "DeletedDateTime",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "RolePermissions");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "RolePermissions");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDateTime", "PermissionName" },
                values: new object[] { new DateTime(2025, 11, 13, 10, 39, 3, 269, DateTimeKind.Utc).AddTicks(9182), "ListPokemon" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDateTime", "PermissionName" },
                values: new object[] { new DateTime(2025, 11, 13, 10, 39, 3, 269, DateTimeKind.Utc).AddTicks(9183), "AddPokemon" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDateTime", "PermissionName" },
                values: new object[] { new DateTime(2025, 11, 13, 10, 39, 3, 269, DateTimeKind.Utc).AddTicks(9183), "UpdatePokemon" });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDateTime", "PermissionName" },
                values: new object[] { new DateTime(2025, 11, 13, 10, 39, 3, 269, DateTimeKind.Utc).AddTicks(9184), "DeletePokemon" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 10, 39, 3, 269, DateTimeKind.Utc).AddTicks(9048));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 10, 39, 3, 269, DateTimeKind.Utc).AddTicks(9051));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 10, 39, 3, 269, DateTimeKind.Utc).AddTicks(9051));
        }
    }
}
