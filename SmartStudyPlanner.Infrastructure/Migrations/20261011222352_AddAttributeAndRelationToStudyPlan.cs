using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStudyPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAttributeAndRelationToStudyPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudyPlanId",
                table: "StudySessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "StudyPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "StudyPlans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_StudySessions_StudyPlanId",
                table: "StudySessions",
                column: "StudyPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudySessions_StudyPlans_StudyPlanId",
                table: "StudySessions",
                column: "StudyPlanId",
                principalTable: "StudyPlans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudySessions_StudyPlans_StudyPlanId",
                table: "StudySessions");

            migrationBuilder.DropIndex(
                name: "IX_StudySessions_StudyPlanId",
                table: "StudySessions");

            migrationBuilder.DropColumn(
                name: "StudyPlanId",
                table: "StudySessions");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "StudyPlans");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "StudyPlans");
        }
    }
}
