using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jobify.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixedModelRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobListings_Users_EmployerId",
                table: "JobListings");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "JobListings",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobListings_UserId",
                table: "JobListings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobListings_Employers_EmployerId",
                table: "JobListings",
                column: "EmployerId",
                principalTable: "Employers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobListings_Users_UserId",
                table: "JobListings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobListings_Employers_EmployerId",
                table: "JobListings");

            migrationBuilder.DropForeignKey(
                name: "FK_JobListings_Users_UserId",
                table: "JobListings");

            migrationBuilder.DropIndex(
                name: "IX_JobListings_UserId",
                table: "JobListings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "JobListings");

            migrationBuilder.AddForeignKey(
                name: "FK_JobListings_Users_EmployerId",
                table: "JobListings",
                column: "EmployerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
