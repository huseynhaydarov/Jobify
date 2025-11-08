using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jobify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedGuestRoleInSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7db11503-a756-4e92-872f-d18c0aa963b2"));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2a9abd5b-36c2-4dad-abac-953b6b4b03be"),
                column: "Name",
                value: "JobSeeker");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2a9abd5b-36c2-4dad-abac-953b6b4b03be"),
                column: "Name",
                value: "Job Seeker");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "CreatedById", "Description", "IsActive", "ModifiedAt", "ModifiedBy", "ModifiedById", "Name" },
                values: new object[] { new Guid("7db11503-a756-4e92-872f-d18c0aa963b2"), new DateTimeOffset(new DateTime(2025, 10, 28, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 5, 0, 0, 0)), null, null, "Guest of the system", true, null, null, null, "Guest" });
        }
    }
}
