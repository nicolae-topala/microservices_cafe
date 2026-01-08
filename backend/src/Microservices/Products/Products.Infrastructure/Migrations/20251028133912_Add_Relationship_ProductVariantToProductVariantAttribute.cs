using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Products.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Relationship_ProductVariantToProductVariantAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantAttribute_ProductVariants_AttributeDefinitionId",
                table: "ProductVariantAttribute");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariantAttribute_UnitsOfMeasureId",
                table: "ProductVariantAttribute");

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitsOfMeasureId",
                table: "ProductVariantAttribute",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductVariantId",
                table: "ProductVariantAttribute",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttribute_ProductVariantId",
                table: "ProductVariantAttribute",
                column: "ProductVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttribute_UnitsOfMeasureId",
                table: "ProductVariantAttribute",
                column: "UnitsOfMeasureId",
                unique: true,
                filter: "[UnitsOfMeasureId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantAttribute_ProductVariants_ProductVariantId",
                table: "ProductVariantAttribute",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariantAttribute_ProductVariants_ProductVariantId",
                table: "ProductVariantAttribute");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariantAttribute_ProductVariantId",
                table: "ProductVariantAttribute");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariantAttribute_UnitsOfMeasureId",
                table: "ProductVariantAttribute");

            migrationBuilder.DropColumn(
                name: "ProductVariantId",
                table: "ProductVariantAttribute");

            migrationBuilder.AlterColumn<Guid>(
                name: "UnitsOfMeasureId",
                table: "ProductVariantAttribute",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttribute_UnitsOfMeasureId",
                table: "ProductVariantAttribute",
                column: "UnitsOfMeasureId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariantAttribute_ProductVariants_AttributeDefinitionId",
                table: "ProductVariantAttribute",
                column: "AttributeDefinitionId",
                principalTable: "ProductVariants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
