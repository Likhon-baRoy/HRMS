export interface UserProfile {
  userId: number;

  employeeId: number;

  username: string;

  roleId: number;

  role: string;

  employeeCode: string;

  fullName: string;

  email: string;

  phone: string;

  address: string;

  hireDate: string;

  employmentType: string;

  employeeStatus: string;

  accountNumber?: string;

  departmentName: string;

  positionTitle: string;

  basicSalary?: number;

  grossSalary?: number;

  salaryEffectiveDate?: string;
}
