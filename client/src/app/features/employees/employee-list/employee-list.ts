import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { EmployeeService } from '../employee.service';
import { Employee } from '../employee.model';
import { EmployeeForm } from '../employee-form/employee-form';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatDialogModule
  ],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.scss'
})
export class EmployeeList implements OnInit {
  private readonly employeeService = inject(EmployeeService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialog = inject(MatDialog);

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

  employees: Employee[] = [];

  ngOnInit(): void {
    this.loadEmployees();
  }

  loadEmployees(): void {
    this.employeeService.getAll().subscribe({
      next: (response) => this.employees = response.items,
      error: () => this.showSnackBar('Failed to load Employees')
    });
  }

  // Base method for dialog handling
  private openForm(employee?: Employee): void {
    const dialogRef = this.dialog.open(EmployeeForm, {
      data: employee,
      width: '700px'
    });

    dialogRef.afterClosed().subscribe((isSaved) => {
      if (isSaved) {
        this.loadEmployees();
      }
    });
  }

  create(): void {
    this.openForm();
  }

  edit(employee: Employee): void {
    this.openForm(employee);
  }

  delete(id: number): void {
    if (!confirm('Are you sure you want to delete this employee?')) {
      return;
    }

    this.employeeService.delete(id).subscribe({
      next: () => {
        this.showSnackBar('Employee deleted successfully');
        this.loadEmployees();
      },
      error: (err) => {
        let message = 'Delete failed';
        const errors = err.error?.errors;
        if (errors) {
          const firstKey = Object.keys(errors)[0];
          if (firstKey && errors[firstKey] && errors[firstKey][0]) {
            message = errors[firstKey][0];
          }
        }
        this.showSnackBar(message);
      }
    });
  }

  private showSnackBar(message: string): void {
    this.snackBar.open(message, 'Close', { duration: 3000 });
  }
}
