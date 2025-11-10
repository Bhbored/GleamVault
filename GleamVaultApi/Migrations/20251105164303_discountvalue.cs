using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GleamVaultApi.Migrations
{
    /// <inheritdoc />
    public partial class discountvalue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "DiscountValue",
                table: "Transaction",
                type: "real",
                nullable: true,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountValue",
                table: "Transaction");
        }
    }
}
