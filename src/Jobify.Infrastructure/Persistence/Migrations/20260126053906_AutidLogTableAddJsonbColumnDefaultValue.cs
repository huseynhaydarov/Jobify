using System.Collections.Generic;
using Jobify.Domain.Entities;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jobify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AutidLogTableAddJsonbColumnDefaultValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<AuditLogDetail>>(
                name: "AuditLogDetails",
                table: "AuditLogs",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'[]'::jsonb",
                oldClrType: typeof(List<AuditLogDetail>),
                oldType: "jsonb");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<AuditLogDetail>>(
                name: "AuditLogDetails",
                table: "AuditLogs",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(List<AuditLogDetail>),
                oldType: "jsonb",
                oldDefaultValueSql: "'[]'::jsonb");
        }
    }
}
