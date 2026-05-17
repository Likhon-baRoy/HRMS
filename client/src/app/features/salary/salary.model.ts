export interface Salary {
  id: number;

  employeeId: number;

  employeeName: string;

  basicSalary: number;

  houseRent: number;

  medicalAllowance: number;

  transportAllowance: number;

  otherAllowance: number;

  grossSalary: number;

  effectiveDate: string;

  isCurrent: boolean;
}
