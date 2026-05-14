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
  private readonly employeeService =
    inject(EmployeeService);

  private readonly snackBar =
    inject(MatSnackBar);

  employees: Employee[] = [];

  displayedColumns: string[] = [
    'employeeCode',
    'name',
    'email',
    'phone',
    'department',
    'position',
    'status',
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

  editEmployee(
    employee: Employee
  ): void {
    console.log(
      'Edit',
      employee
    );
  }

  deleteEmployee(
    id: number
  ): void {
    const confirmed =
      confirm(
        'Are you sure you want to delete this employee?'
      );

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

        error: () => {
          this.snackBar.open(
            'Failed to delete employee',
            'Close',
            {
              duration: 3000
            }
          );
        }
      });
  }
}
