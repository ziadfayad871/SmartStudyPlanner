using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStudyPlanner.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeStudyPlanIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudySessions_StudyPlans_StudyPlanId",
                table: "StudySessions");

            migrationBuilder.AlterColumn<int>(
                name: "StudyPlanId",
                table: "StudySessions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "StudyPlans",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_StudySessions_StudyPlans_StudyPlanId",
                table: "StudySessions",
                column: "StudyPlanId",
                principalTable: "StudyPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudySessions_StudyPlans_StudyPlanId",
                table: "StudySessions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "StudyPlans");

            migrationBuilder.AlterColumn<int>(
                name: "StudyPlanId",
                table: "StudySessions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudySessions_StudyPlans_StudyPlanId",
                table: "StudySessions",
                column: "StudyPlanId",
                principalTable: "StudyPlans",
                principalColumn: "Id");
        }
    }
}
