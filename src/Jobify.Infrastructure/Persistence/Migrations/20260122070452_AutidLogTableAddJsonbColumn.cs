using System.Collections.Generic;
using Jobify.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jobify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AutidLogTableAddJsonbColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Changes",
                table: "AuditLogs");

            migrationBuilder.AddColumn<List<AuditLogDetail>>(
                name: "AuditLogDetails",
                table: "AuditLogs",
                type: "jsonb",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditLogDetails",
                table: "AuditLogs");

            migrationBuilder.AddColumn<string>(
                name: "Changes",
                table: "AuditLogs",
                type: "text",
                nullable: true);
        }
    }
}
