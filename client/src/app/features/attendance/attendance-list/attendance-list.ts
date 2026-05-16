import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import {
  MatSnackBar,
  MatSnackBarModule,
} from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';

import { AttendanceService } from '../../../core/services/attendance.service';
import { Attendance } from '../../../core/models/attendance.model';

@Component({
  selector: 'app-attendance-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatSnackBarModule,
  ],
  templateUrl: './attendance-list.html',
  styleUrl: './attendance-list.scss',
})
export class AttendanceList implements OnInit {
  private readonly service = inject(AttendanceService);
  private readonly snackBar = inject(MatSnackBar);

  attendances: Attendance[] = [];

  displayedColumns: string[] = [
    'employee',
    'date',
    'checkIn',
    'checkOut',
    'status',
    'remarks',
  ];

  ngOnInit(): void {
    this.loadAttendance();
  }

  loadAttendance(): void {
    this.service.getAll().subscribe({
      next: (response) => {
        this.attendances = response.items;
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

  checkIn(): void {
    this.service.checkIn('Arrived at office').subscribe({
      next: () => {
        this.snackBar.open(
          'Checked in',
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
}