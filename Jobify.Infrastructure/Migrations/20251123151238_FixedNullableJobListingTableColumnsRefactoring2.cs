using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jobify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixedNullableJobListingTableColumnsRefactoring2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Salary",
                table: "JobListings",
                type: "numeric(14,2)",
                precision: 14,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(14,2)",
                oldPrecision: 14,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Salary",
                table: "JobListings",
                type: "numeric(14,2)",
                precision: 14,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(14,2)",
                oldPrecision: 14,
                oldScale: 2,
                oldNullable: true);
        }
    }
}
