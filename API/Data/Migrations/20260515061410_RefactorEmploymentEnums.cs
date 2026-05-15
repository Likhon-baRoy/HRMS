using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactorEmploymentEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmploymentStatus",
                table: "employees",
                newName: "EmploymentType");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeStatus",
                table: "employees",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeStatus",
                table: "employees");

            migrationBuilder.RenameColumn(
                name: "EmploymentType",
                table: "employees",
                newName: "EmploymentStatus");
        }
    }
}
