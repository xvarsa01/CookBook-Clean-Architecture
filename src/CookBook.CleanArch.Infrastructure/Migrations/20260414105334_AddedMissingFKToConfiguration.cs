using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookBook.CleanArch.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedMissingFKToConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientInRecipe_Ingredients_IngredientId",
                table: "IngredientInRecipe");

            migrationBuilder.AlterColumn<double>(
                name: "Duration",
                table: "Recipes",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientInRecipe_Ingredients_IngredientId",
                table: "IngredientInRecipe",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientInRecipe_Ingredients_IngredientId",
                table: "IngredientInRecipe");

            migrationBuilder.AlterColumn<double>(
                name: "Duration",
                table: "Recipes",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientInRecipe_Ingredients_IngredientId",
                table: "IngredientInRecipe",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
