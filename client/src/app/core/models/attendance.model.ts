export interface Attendance {
  id: number;

  employeeId: number;

  employeeName: string;

  date: string;

  checkInTime: string;

  checkOutTime?: string;

  statusId: number;

  status: string;

  remarks?: string;
}