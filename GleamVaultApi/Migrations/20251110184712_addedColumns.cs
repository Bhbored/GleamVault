using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GleamVaultApi.Migrations
{
    /// <inheritdoc />
    public partial class addedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Quantity",
                table: "TransactionItem",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "TransactionItem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "TransactionItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Hallmark",
                table: "TransactionItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "TransactionItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsUniquePiece",
                table: "TransactionItem",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "TransactionItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "OfferPrice",
                table: "TransactionItem",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sku",
                table: "TransactionItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "TransactionItem",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WeightUnit",
                table: "TransactionItem",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Transaction",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "SubTotalAmount",
                table: "Transaction",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AlterColumn<float>(
                name: "Weight",
                table: "Product",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AlterColumn<bool>(
                name: "IsUniquePiece",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Product",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "OfferPrice",
                table: "Product",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Customer",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Category",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItem_CategoryId",
                table: "TransactionItem",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionItem_Category_CategoryId",
                table: "TransactionItem",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionItem_Category_CategoryId",
                table: "TransactionItem");

            migrationBuilder.DropIndex(
                name: "IX_TransactionItem_CategoryId",
                table: "TransactionItem");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "TransactionItem");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "TransactionItem");

            migrationBuilder.DropColumn(
                name: "Hallmark",
                table: "TransactionItem");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "TransactionItem");

            migrationBuilder.DropColumn(
                name: "IsUniquePiece",
                table: "TransactionItem");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "TransactionItem");

            migrationBuilder.DropColumn(
                name: "OfferPrice",
                table: "TransactionItem");

            migrationBuilder.DropColumn(
                name: "Sku",
                table: "TransactionItem");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "TransactionItem");

            migrationBuilder.DropColumn(
                name: "WeightUnit",
                table: "TransactionItem");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "SubTotalAmount",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "OfferPrice",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Customer");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Category");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "TransactionItem",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real",
                oldDefaultValue: 0f);

            migrationBuilder.AlterColumn<float>(
                name: "Weight",
                table: "Product",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsUniquePiece",
                table: "Product",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);
        }
    }
}
