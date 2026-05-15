import {
  Component,
  OnInit,
  inject
} from '@angular/core';

import {
  CommonModule
} from '@angular/common';

import {
  MatTableModule
} from '@angular/material/table';

import {
  MatButtonModule
} from '@angular/material/button';

import {
  MatIconModule
} from '@angular/material/icon';

import {
  MatSnackBar,
  MatSnackBarModule
} from '@angular/material/snack-bar';

import {
  EmployeeService
} from '../employee.service';

import {
  Employee
} from '../employee.model';

import {
  MatDialog,
  MatDialogModule
} from '@angular/material/dialog';

import {
  EmployeeForm
} from '../employee-form/employee-form';

@Component({
  selector: 'app-employee-list',

  standalone: true,

  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule
  ],

  templateUrl:
    './employee-list.html',

  styleUrl:
    './employee-list.scss'
})
export class EmployeeList
  implements OnInit {
  private readonly employeeService = inject(EmployeeService);

  private readonly snackBar = inject(MatSnackBar);

  private dialog = inject(MatDialog);

  employees: Employee[] = [];

  displayedColumns: string[] = [
    'employeeCode',
    'name',
    'email',
    'phone',
    'department',
    'position',
    'employmentType',
    'employeeStatus',
    'actions'
  ];

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.employeeService
      .getAll()
      .subscribe({
        next: (response) => {
          this.employees =
            response.items;
        },

        error: () => {
          this.snackBar.open(
            'Failed to load employees',
            'Close',
            {
              duration: 3000
            }
          );
        }
      });
  }

  openForm(employee?: Employee): void {
    const dialogRef =
      this.dialog.open(
        EmployeeForm,
        {
          data: employee,
          width: '700px'
        }
      );

    dialogRef
      .afterClosed()
      .subscribe(result => {
        if (result) {
          this.loadEmployees();
        }
      });
  }

  editEmployee(employee: Employee): void {
    this.openForm(employee);
  }

  deleteEmployee(id: number): void {
    const confirmed =
      confirm('Are you sure you want to delete this employee?');

    if (!confirmed) {
      return;
    }

    this.employeeService
      .delete(id)
      .subscribe({
        next: () => {
          this.snackBar.open(
            'Employee deleted successfully',
            'Close',
            {
              duration: 3000
            }
          );

          this.loadEmployees();
        },

        error: err => {
          let message = 'Delete failed';
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
              duration: 3000
            }
          );
        }
      });
  }
}
