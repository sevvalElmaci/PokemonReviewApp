using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    public partial class CleanUserLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Users");

          

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "UserLogs");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "UserLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 8, 1, 28, 369, DateTimeKind.Utc).AddTicks(4336));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 8, 1, 28, 369, DateTimeKind.Utc).AddTicks(4336));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 8, 1, 28, 369, DateTimeKind.Utc).AddTicks(4337));

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 8, 1, 28, 369, DateTimeKind.Utc).AddTicks(4337));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 8, 1, 28, 369, DateTimeKind.Utc).AddTicks(4208));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 8, 1, 28, 369, DateTimeKind.Utc).AddTicks(4209));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDateTime",
                value: new DateTime(2025, 11, 13, 8, 1, 28, 369, DateTimeKind.Utc).AddTicks(4210));
        }
    }
}
