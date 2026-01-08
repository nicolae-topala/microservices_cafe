using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Products.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OneToManyRelationshipForProductVariantsAndAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductVariantAttribute_AttributeDefinitionId",
                table: "ProductVariantAttribute");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariantAttribute_UnitsOfMeasureId",
                table: "ProductVariantAttribute");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttribute_AttributeDefinitionId",
                table: "ProductVariantAttribute",
                column: "AttributeDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttribute_UnitsOfMeasureId",
                table: "ProductVariantAttribute",
                column: "UnitsOfMeasureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductVariantAttribute_AttributeDefinitionId",
                table: "ProductVariantAttribute");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariantAttribute_UnitsOfMeasureId",
                table: "ProductVariantAttribute");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttribute_AttributeDefinitionId",
                table: "ProductVariantAttribute",
                column: "AttributeDefinitionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttribute_UnitsOfMeasureId",
                table: "ProductVariantAttribute",
                column: "UnitsOfMeasureId",
                unique: true,
                filter: "[UnitsOfMeasureId] IS NOT NULL");
        }
    }
}
