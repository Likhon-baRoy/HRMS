import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';

import { BaseListComponent } from '../../../core/base/base-list.component';
import { Employee } from '../employee.model';
import { EmployeeService } from '../employee.service';
import { EmployeeForm } from '../employee-form/employee-form';
import { DepartmentService } from '../../../core/services/department.service';
import { Department } from '../../../core/models/department.model';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatPaginatorModule
  ],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.scss'
})
export class EmployeeList extends BaseListComponent<Employee> {
  protected service = inject(EmployeeService);
  protected formComponent = EmployeeForm;
  protected entityName = 'Employee';
  protected override dialogWidth = '920px';
  private readonly fb = inject(FormBuilder);
  private readonly departmentService = inject(DepartmentService);
  private readonly authService = inject(AuthService);

  readonly canManageEmployees = ['Admin', 'HR']
    .includes(this.authService.getRole() ?? '');

  departments: Department[] = [];
  statuses = [
    { value: 1, label: 'Active' },
    { value: 2, label: 'Inactive' },
    { value: 3, label: 'Suspended' },
    { value: 4, label: 'Resigned' },
    { value: 5, label: 'Terminated' }
  ];

  filters = this.fb.group({
    search: [''],
    departmentId: [null as number | null],
    employeeStatus: [null as number | null]
  });

  displayedColumns: string[] = this.canManageEmployees
    ? ['employeeCode', 'name', 'email', 'phone', 'department', 'position', 'employmentType', 'employeeStatus', 'actions']
    : ['employeeCode', 'name', 'email', 'phone', 'department', 'position', 'employmentType', 'employeeStatus'];

  override ngOnInit(): void {
    this.loadDepartments();
    super.ngOnInit();
    this.filters.valueChanges.subscribe(() => {
      this.pageIndex = 0;
      this.loadItems();
    });
  }

  override loadItems(): void {
    const value = this.filters.getRawValue();

    this.service.getAll(this.pageIndex + 1, this.pageSize, {
      search: value.search?.trim() || undefined,
      departmentId: value.departmentId,
      employeeStatus: value.employeeStatus
    }).subscribe({
      next: (response) => {
        this.items = response.items;
        this.totalCount = response.meta.totalCount;
      },
      error: (err) => this.showSnackBar(`Failed to load ${this.entityName}s`)
    });
  }

  private loadDepartments(): void {
    this.departmentService.getAll(1, 50).subscribe({
      next: (response) => this.departments = response.items
    });
  }
}
