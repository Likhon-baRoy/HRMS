using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSalaryStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_salaries_employees_EmployeeId",
                table: "salaries");

            migrationBuilder.DropIndex(
                name: "IX_salaries_EmployeeId",
                table: "salaries");

            migrationBuilder.RenameColumn(
                name: "OldSalary",
                table: "SalaryRevisions",
                newName: "OldGrossSalary");

            migrationBuilder.RenameColumn(
                name: "NewSalary",
                table: "SalaryRevisions",
                newName: "NewGrossSalary");

            migrationBuilder.RenameColumn(
                name: "BaseSalary",
                table: "salaries",
                newName: "TransportAllowance");

            migrationBuilder.RenameColumn(
                name: "Allowance",
                table: "salaries",
                newName: "OtherAllowance");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCurrent",
                table: "salaries",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<decimal>(
                name: "BasicSalary",
                table: "salaries",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GrossSalary",
                table: "salaries",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HouseRent",
                table: "salaries",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MedicalAllowance",
                table: "salaries",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_salaries_EmployeeId_IsCurrent",
                table: "salaries",
                columns: new[] { "EmployeeId", "IsCurrent" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_salaries_employees_EmployeeId",
                table: "salaries",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_salaries_employees_EmployeeId",
                table: "salaries");

            migrationBuilder.DropIndex(
                name: "IX_salaries_EmployeeId_IsCurrent",
                table: "salaries");

            migrationBuilder.DropColumn(
                name: "BasicSalary",
                table: "salaries");

            migrationBuilder.DropColumn(
                name: "GrossSalary",
                table: "salaries");

            migrationBuilder.DropColumn(
                name: "HouseRent",
                table: "salaries");

            migrationBuilder.DropColumn(
                name: "MedicalAllowance",
                table: "salaries");

            migrationBuilder.RenameColumn(
                name: "OldGrossSalary",
                table: "SalaryRevisions",
                newName: "OldSalary");

            migrationBuilder.RenameColumn(
                name: "NewGrossSalary",
                table: "SalaryRevisions",
                newName: "NewSalary");

            migrationBuilder.RenameColumn(
                name: "TransportAllowance",
                table: "salaries",
                newName: "BaseSalary");

            migrationBuilder.RenameColumn(
                name: "OtherAllowance",
                table: "salaries",
                newName: "Allowance");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCurrent",
                table: "salaries",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_salaries_EmployeeId",
                table: "salaries",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_salaries_employees_EmployeeId",
                table: "salaries",
                column: "EmployeeId",
                principalTable: "employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
