export interface Payroll {
  id: number;

  employeeId: number;

  employeeName: string;

  payPeriodStart: string;

  payPeriodEnd: string;

  grossSalary: number;

  totalBonus: number;

  totalDeductions: number;

  taxAmount: number;

  netSalary: number;

  statusId: number;

  status: string;

  generatedAt: string;
}
