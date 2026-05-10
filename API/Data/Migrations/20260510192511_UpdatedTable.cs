using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bonus_payrolls_PayrollId",
                table: "Bonus");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollDeduction_Deduction_DeductionId",
                table: "PayrollDeduction");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollDeduction_payrolls_PayrollId",
                table: "PayrollDeduction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayrollDeduction",
                table: "PayrollDeduction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deduction",
                table: "Deduction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bonus",
                table: "Bonus");

            migrationBuilder.RenameTable(
                name: "PayrollDeduction",
                newName: "PayrollDeductions");

            migrationBuilder.RenameTable(
                name: "Deduction",
                newName: "Deductions");

            migrationBuilder.RenameTable(
                name: "Bonus",
                newName: "Bonuses");

            migrationBuilder.RenameIndex(
                name: "IX_PayrollDeduction_PayrollId",
                table: "PayrollDeductions",
                newName: "IX_PayrollDeductions_PayrollId");

            migrationBuilder.RenameIndex(
                name: "IX_PayrollDeduction_DeductionId",
                table: "PayrollDeductions",
                newName: "IX_PayrollDeductions_DeductionId");

            migrationBuilder.RenameIndex(
                name: "IX_Bonus_PayrollId",
                table: "Bonuses",
                newName: "IX_Bonuses_PayrollId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayrollDeductions",
                table: "PayrollDeductions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deductions",
                table: "Deductions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bonuses",
                table: "Bonuses",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SalaryRevisions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SalaryId = table.Column<int>(type: "int", nullable: false),
                    OldSalary = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    NewSalary = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Reason = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RevisionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ApprovedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryRevisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryRevisions_salaries_SalaryId",
                        column: x => x.SalaryId,
                        principalTable: "salaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryRevisions_SalaryId",
                table: "SalaryRevisions",
                column: "SalaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bonuses_payrolls_PayrollId",
                table: "Bonuses",
                column: "PayrollId",
                principalTable: "payrolls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollDeductions_Deductions_DeductionId",
                table: "PayrollDeductions",
                column: "DeductionId",
                principalTable: "Deductions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollDeductions_payrolls_PayrollId",
                table: "PayrollDeductions",
                column: "PayrollId",
                principalTable: "payrolls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bonuses_payrolls_PayrollId",
                table: "Bonuses");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollDeductions_Deductions_DeductionId",
                table: "PayrollDeductions");

            migrationBuilder.DropForeignKey(
                name: "FK_PayrollDeductions_payrolls_PayrollId",
                table: "PayrollDeductions");

            migrationBuilder.DropTable(
                name: "SalaryRevisions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PayrollDeductions",
                table: "PayrollDeductions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deductions",
                table: "Deductions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bonuses",
                table: "Bonuses");

            migrationBuilder.RenameTable(
                name: "PayrollDeductions",
                newName: "PayrollDeduction");

            migrationBuilder.RenameTable(
                name: "Deductions",
                newName: "Deduction");

            migrationBuilder.RenameTable(
                name: "Bonuses",
                newName: "Bonus");

            migrationBuilder.RenameIndex(
                name: "IX_PayrollDeductions_PayrollId",
                table: "PayrollDeduction",
                newName: "IX_PayrollDeduction_PayrollId");

            migrationBuilder.RenameIndex(
                name: "IX_PayrollDeductions_DeductionId",
                table: "PayrollDeduction",
                newName: "IX_PayrollDeduction_DeductionId");

            migrationBuilder.RenameIndex(
                name: "IX_Bonuses_PayrollId",
                table: "Bonus",
                newName: "IX_Bonus_PayrollId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PayrollDeduction",
                table: "PayrollDeduction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deduction",
                table: "Deduction",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bonus",
                table: "Bonus",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bonus_payrolls_PayrollId",
                table: "Bonus",
                column: "PayrollId",
                principalTable: "payrolls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollDeduction_Deduction_DeductionId",
                table: "PayrollDeduction",
                column: "DeductionId",
                principalTable: "Deduction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PayrollDeduction_payrolls_PayrollId",
                table: "PayrollDeduction",
                column: "PayrollId",
                principalTable: "payrolls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
