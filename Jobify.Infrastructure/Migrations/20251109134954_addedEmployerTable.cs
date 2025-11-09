using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jobify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedEmployerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2a9abd5b-36c2-4dad-abac-953b6b4b03be"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 10, 28, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 0, 0, 0)), null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("bb176f73-41a2-4b9d-b85c-3805e8d8ee12"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 10, 28, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 0, 0, 0)), null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cdb9e288-36c7-4ae0-b517-476d9cd0224b"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTimeOffset(new DateTime(2025, 10, 28, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 0, 0, 0)), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2a9abd5b-36c2-4dad-abac-953b6b4b03be"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 10, 28, 12, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("bb176f73-41a2-4b9d-b85c-3805e8d8ee12"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 10, 28, 12, 0, 0, 0, DateTimeKind.Unspecified), null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cdb9e288-36c7-4ae0-b517-476d9cd0224b"),
                columns: new[] { "CreatedAt", "ModifiedAt" },
                values: new object[] { new DateTime(2025, 10, 28, 12, 0, 0, 0, DateTimeKind.Unspecified), null });
        }
    }
}
