import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';

import { getApiError } from '../../../core/utils/error-handler.util';
import { Employee } from '../../employees/employee.model';
import { EmployeeService } from '../../employees/employee.service';
import { UserAccount, UserAccountService } from '../user-account.service';

@Component({
  selector: 'app-user-account-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatSnackBarModule,
    MatTableModule,
  ],
  templateUrl: './user-account-list.html',
  styleUrl: './user-account-list.scss',
})
export class UserAccountList implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly employeeService = inject(EmployeeService);
  private readonly userAccountService = inject(UserAccountService);
  private readonly snackBar = inject(MatSnackBar);

  employees: Employee[] = [];

  users: UserAccount[] = [];

  displayedColumns = [
    'employee',
    'username',
    'role',
    'actions'
  ];

  readonly roles = [
    { value: 1, label: 'Admin' },
    { value: 2, label: 'HR' },
    { value: 3, label: 'Manager' },
    { value: 4, label: 'Employee' },
  ];

  readonly form = this.fb.group({
    employeeId: [null as number | null, Validators.required],
    username: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    role: [4, Validators.required],
  });

  ngOnInit(): void {
    this.loadEmployees();
    this.loadUsers();
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.getRawValue();

    this.userAccountService.create({
      employeeId: Number(raw.employeeId),
      username: raw.username ?? '',
      password: raw.password ?? '',
      role: Number(raw.role),
    }).subscribe({
      next: () => {
        this.snackBar.open(
          'User account created',
          'Close',
          { duration: 3000 }
        );

        this.form.reset({
          employeeId: null,
          username: '',
          password: '',
          role: 4,
        });

        this.loadUsers();
      },
      error: (err) => {
        this.snackBar.open(
          getApiError(err, 'Operation failed'),
          'Close',
          { duration: 3000 }
        );
      },
    });
  }

  updateRole(user: UserAccount, role: number): void {
    if (user.roleId === role) {
      return;
    }

    this.userAccountService.updateRole(user.id, role).subscribe({
      next: () => {
        this.snackBar.open(
          'Role updated',
          'Close',
          { duration: 3000 }
        );

        this.loadUsers();
      },
      error: (err) => {
        this.snackBar.open(
          getApiError(err, 'Role update failed'),
          'Close',
          { duration: 3000 }
        );

        this.loadUsers();
      }
    });
  }

  deleteUser(user: UserAccount): void {
    if (!confirm(`Delete user account "${user.username}"?`)) {
      return;
    }

    this.userAccountService.delete(user.id).subscribe({
      next: () => {
        this.snackBar.open(
          'User account deleted',
          'Close',
          { duration: 3000 }
        );

        this.loadUsers();
        this.loadEmployees();
      },
      error: (err) => {
        this.snackBar.open(
          getApiError(err, 'Delete failed'),
          'Close',
          { duration: 3000 }
        );
      }
    });
  }

  private loadEmployees(): void {
    this.employeeService.getAll(1, 100).subscribe({
      next: (response) => {
        this.employees = response.items;
      },
      error: () => {
        this.snackBar.open(
          'Failed to load employees',
          'Close',
          { duration: 3000 }
        );
      },
    });
  }

  private loadUsers(): void {
    this.userAccountService.getAll().subscribe({
      next: (response) => {
        this.users = response.data;
      },
      error: () => {
        this.snackBar.open(
          'Failed to load user accounts',
          'Close',
          { duration: 3000 }
        );
      }
    });
  }
}
