using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePayrollIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_payrolls_EmployeeId_PayPeriodStart_PayPeriodEnd_IsDeleted",
                table: "payrolls",
                columns: new[] { "EmployeeId", "PayPeriodStart", "PayPeriodEnd", "IsDeleted" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_payrolls_EmployeeId_PayPeriodStart_PayPeriodEnd_IsDeleted",
                table: "payrolls");
        }
    }
}
