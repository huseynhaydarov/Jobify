using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jobify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixSeederData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2a9abd5b-36c2-4dad-abac-953b6b4b03be"),
                column: "Description",
                value: "JobSeeker of the System");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2a9abd5b-36c2-4dad-abac-953b6b4b03be"),
                column: "Description",
                value: "Guest of the System");
        }
    }
}
