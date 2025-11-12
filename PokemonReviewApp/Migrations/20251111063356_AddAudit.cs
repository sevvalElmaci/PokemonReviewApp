using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokemonReviewApp.Migrations
{
    public partial class AddAudit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Reviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDateTime",
                table: "Reviews",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Reviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Reviews",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedUserId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Reviewers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserId",
                table: "Reviewers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDateTime",
                table: "Reviewers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "Reviewers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Reviewers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Reviewers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedUserId",
                table: "Reviewers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Properties",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserId",
                table: "Properties",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDateTime",
                table: "Properties",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "Properties",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Properties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Properties",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedUserId",
                table: "Properties",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "PokeProperties",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserId",
                table: "PokeProperties",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDateTime",
                table: "PokeProperties",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "PokeProperties",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PokeProperties",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "PokeProperties",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedUserId",
                table: "PokeProperties",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "PokemonOwners",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserId",
                table: "PokemonOwners",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDateTime",
                table: "PokemonOwners",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "PokemonOwners",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PokemonOwners",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "PokemonOwners",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedUserId",
                table: "PokemonOwners",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "PokemonCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserId",
                table: "PokemonCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDateTime",
                table: "PokemonCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "PokemonCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PokemonCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "PokemonCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedUserId",
                table: "PokemonCategories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Pokemon",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserId",
                table: "Pokemon",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDateTime",
                table: "Pokemon",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "Pokemon",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Pokemon",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Pokemon",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedUserId",
                table: "Pokemon",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "PokeFoods",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserId",
                table: "PokeFoods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDateTime",
                table: "PokeFoods",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "PokeFoods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "PokeFoods",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "PokeFoods",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedUserId",
                table: "PokeFoods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Owners",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserId",
                table: "Owners",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDateTime",
                table: "Owners",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "Owners",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Owners",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Owners",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedUserId",
                table: "Owners",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Foods",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserId",
                table: "Foods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDateTime",
                table: "Foods",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "Foods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Foods",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Foods",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedUserId",
                table: "Foods",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Countries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserId",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDateTime",
                table: "Countries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Countries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Countries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedUserId",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDateTime",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedUserId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDateTime",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedUserId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDateTime",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedUserId",
                table: "Categories",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "DeletedDateTime",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Reviewers");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Reviewers");

            migrationBuilder.DropColumn(
                name: "DeletedDateTime",
                table: "Reviewers");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Reviewers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Reviewers");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Reviewers");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "Reviewers");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "DeletedDateTime",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "PokeProperties");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "PokeProperties");

            migrationBuilder.DropColumn(
                name: "DeletedDateTime",
                table: "PokeProperties");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "PokeProperties");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PokeProperties");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "PokeProperties");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "PokeProperties");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "PokemonOwners");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "PokemonOwners");

            migrationBuilder.DropColumn(
                name: "DeletedDateTime",
                table: "PokemonOwners");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "PokemonOwners");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PokemonOwners");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "PokemonOwners");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "PokemonOwners");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "PokemonCategories");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "PokemonCategories");

            migrationBuilder.DropColumn(
                name: "DeletedDateTime",
                table: "PokemonCategories");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "PokemonCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PokemonCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "PokemonCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "PokemonCategories");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Pokemon");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Pokemon");

            migrationBuilder.DropColumn(
                name: "DeletedDateTime",
                table: "Pokemon");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Pokemon");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Pokemon");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Pokemon");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "Pokemon");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "PokeFoods");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "PokeFoods");

            migrationBuilder.DropColumn(
                name: "DeletedDateTime",
                table: "PokeFoods");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "PokeFoods");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "PokeFoods");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "PokeFoods");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "PokeFoods");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "DeletedDateTime",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "DeletedDateTime",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "DeletedDateTime",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CreatedDateTime",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedDateTime",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedDateTime",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "Categories");
        }
    }
}
