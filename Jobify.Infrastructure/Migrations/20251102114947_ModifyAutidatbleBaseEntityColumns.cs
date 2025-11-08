using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jobify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyAutidatbleBaseEntityColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2a9abd5b-36c2-4dad-abac-953b6b4b03be"),
                column: "ModifiedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7db11503-a756-4e92-872f-d18c0aa963b2"),
                column: "ModifiedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("bb176f73-41a2-4b9d-b85c-3805e8d8ee12"),
                column: "ModifiedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cdb9e288-36c7-4ae0-b517-476d9cd0224b"),
                column: "ModifiedAt",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("2a9abd5b-36c2-4dad-abac-953b6b4b03be"),
                column: "ModifiedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("7db11503-a756-4e92-872f-d18c0aa963b2"),
                column: "ModifiedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("bb176f73-41a2-4b9d-b85c-3805e8d8ee12"),
                column: "ModifiedAt",
                value: null);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("cdb9e288-36c7-4ae0-b517-476d9cd0224b"),
                column: "ModifiedAt",
                value: null);
        }
    }
}
