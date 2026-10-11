using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStudyPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskDueDateAndStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_UserId",
                table: "Tasks",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_UserId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tasks");
        }
    }
}
