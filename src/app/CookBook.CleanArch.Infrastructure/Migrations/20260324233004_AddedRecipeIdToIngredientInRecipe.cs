using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookBook.Clean.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedRecipeIdToIngredientInRecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientInRecipe",
                table: "IngredientInRecipe");

            migrationBuilder.DropIndex(
                name: "IX_IngredientInRecipe_RecipeId",
                table: "IngredientInRecipe");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecipeId",
                table: "IngredientInRecipe",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientInRecipe",
                table: "IngredientInRecipe",
                columns: new[] { "RecipeId", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientInRecipe",
                table: "IngredientInRecipe");

            migrationBuilder.AlterColumn<Guid>(
                name: "RecipeId",
                table: "IngredientInRecipe",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "TEXT");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientInRecipe",
                table: "IngredientInRecipe",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientInRecipe_RecipeId",
                table: "IngredientInRecipe",
                column: "RecipeId");
        }
    }
}
