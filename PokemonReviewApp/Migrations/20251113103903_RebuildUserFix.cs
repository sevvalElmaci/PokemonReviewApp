using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    public partial class RebuildUserFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 10, 39, 3, 269, DateTimeKind.Utc).AddTicks(9182));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 10, 39, 3, 269, DateTimeKind.Utc).AddTicks(9183));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 10, 39, 3, 269, DateTimeKind.Utc).AddTicks(9183));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 10, 39, 3, 269, DateTimeKind.Utc).AddTicks(9184));

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 10, 37, 16, 274, DateTimeKind.Utc).AddTicks(9651));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 10, 37, 16, 274, DateTimeKind.Utc).AddTicks(9652));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 10, 37, 16, 274, DateTimeKind.Utc).AddTicks(9652));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 10, 37, 16, 274, DateTimeKind.Utc).AddTicks(9652));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 10, 37, 16, 274, DateTimeKind.Utc).AddTicks(9479));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 10, 37, 16, 274, DateTimeKind.Utc).AddTicks(9481));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 10, 37, 16, 274, DateTimeKind.Utc).AddTicks(9482));
        }
    }
}
