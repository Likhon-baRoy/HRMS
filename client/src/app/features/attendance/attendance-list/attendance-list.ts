import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import {
  MatSnackBar,
  MatSnackBarModule,
} from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';

import { AttendanceService } from '../../../core/services/attendance.service';
import { Attendance } from '../../../core/models/attendance.model';
import { AuthService } from '../../../core/services/auth.service';
import { EmployeeService } from '../../employees/employee.service';
import { Employee } from '../../employees/employee.model';

@Component({
  selector: 'app-attendance-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatTableModule,
    MatButtonModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatSnackBarModule,
  ],
  templateUrl: './attendance-list.html',
  styleUrl: './attendance-list.scss',
})
export class AttendanceList implements OnInit {
  private readonly service = inject(AttendanceService);
  private readonly fb = inject(FormBuilder);
  private readonly snackBar = inject(MatSnackBar);
  private readonly authService = inject(AuthService);
  private readonly employeeService = inject(EmployeeService);

  attendances: Attendance[] = [];
  employees: Employee[] = [];

  remarks = '';
  pageIndex = 0;
  pageSize = 10;
  totalCount = 0;
  readonly pageSizeOptions = [5, 10, 25];

  filters = this.fb.group({
    employeeId: [null as number | null],
    attendanceDate: [''],
  });

  readonly isEmployee = this.authService.isEmployee();

  readonly displayedColumns: string[] = this.isEmployee
    ? [
        'date',
        'checkIn',
        'checkOut',
        'status',
        'remarks',
      ]
    : [
        'employee',
        'date',
        'checkIn',
        'checkOut',
        'status',
        'remarks',
      ];

  ngOnInit(): void {
    if (!this.isEmployee) {
      this.loadEmployees();
    }

    this.filters.valueChanges.subscribe(() => {
      this.pageIndex = 0;
      this.loadAttendance();
    });

    this.loadAttendance();
  }

  loadAttendance(): void {
    const value = this.filters.getRawValue();

    this.service.getAll(this.pageIndex + 1, this.pageSize, {
      employeeId: this.isEmployee ? null : value.employeeId,
      attendanceDate: value.attendanceDate || null,
    }).subscribe({
      next: (response) => {
        this.attendances = response.items;
        this.totalCount = response.meta.totalCount;
      },

      error: () => {
        this.snackBar.open(
          'Failed to load attendance',
          'Close',
          {
            duration: 3000,
          }
        );
      },
    });
  }

  onPageChange(event: { pageIndex: number; pageSize: number }): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadAttendance();
  }

  checkIn(): void {
    this.service.checkIn(this.remarks.trim()).subscribe({
      next: () => {
        this.snackBar.open(
          'Checked in',
          'Close',
          {
            duration: 3000,
          }
        );

        this.remarks = '';
        this.loadAttendance();
      },

      error: (err) => {
        this.showError(err);
      },
    });
  }

  checkOut(): void {
    this.service.checkOut().subscribe({
      next: () => {
        this.snackBar.open(
          'Checked out',
          'Close',
          {
            duration: 3000,
          }
        );

        this.loadAttendance();
      },

      error: (err) => {
        this.showError(err);
      },
    });
  }

  showError(err: any): void {
    let message = 'Operation failed';

    const errors = err.error?.errors;

    if (errors) {
      const firstKey = Object.keys(errors)[0];

      if (firstKey) {
        message = errors[firstKey][0];
      }
    }

    this.snackBar.open(
      message,
      'Close',
      {
        duration: 3000,
      }
    );
  }

  private loadEmployees(): void {
    this.employeeService.getAll(1, 50).subscribe({
      next: (response) => {
        this.employees = response.items;
      },
    });
  }
}
