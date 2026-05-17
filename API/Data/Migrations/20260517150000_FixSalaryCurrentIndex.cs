using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class FixSalaryCurrentIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_salaries_EmployeeId_IsCurrent",
                table: "salaries");

            migrationBuilder.CreateIndex(
                name: "IX_salaries_EmployeeId",
                table: "salaries",
                column: "EmployeeId",
                unique: true,
                filter: "\"IsCurrent\" = true");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_salaries_EmployeeId",
                table: "salaries");

            migrationBuilder.CreateIndex(
                name: "IX_salaries_EmployeeId_IsCurrent",
                table: "salaries",
                columns: new[] { "EmployeeId", "IsCurrent" },
                unique: true);
        }
    }
}
