import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';

import { BaseListComponent } from '../../../core/base/base-list.component';
import { Employee } from '../../employees/employee.model';
import { EmployeeService } from '../../employees/employee.service';
import { Salary } from '../salary.model';
import { SalaryService } from '../salary.service';
import { SalaryForm } from '../salary-form/salary-form';

@Component({
  selector: 'app-salary-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatDialogModule,
    MatSnackBarModule,
    MatPaginatorModule,
    MatSelectModule,
  ],
  templateUrl: './salary-list.html',
  styleUrl: './salary-list.scss',
})
export class SalaryList extends BaseListComponent<Salary> {
  protected service = inject(SalaryService);
  protected formComponent = SalaryForm;
  protected entityName = 'Salary';
  private readonly fb = inject(FormBuilder);
  private readonly employeeService = inject(EmployeeService);

  employees: Employee[] = [];

  filters = this.fb.group({
    employeeId: [null as number | null]
  });

  displayedColumns: string[] = [
    'employee',
    'basicSalary',
    'houseRent',
    'medicalAllowance',
    'transportAllowance',
    'grossSalary',
    'effectiveDate',
    'actions',
  ];

  override ngOnInit(): void {
    this.loadEmployees();
    super.ngOnInit();

    this.filters.valueChanges.subscribe(() => {
      this.pageIndex = 0;
      this.loadItems();
    });
  }

  override loadItems(): void {
    const value = this.filters.getRawValue();

    this.service.getAll(this.pageIndex + 1, this.pageSize, {
      employeeId: value.employeeId
    }).subscribe({
      next: (response) => {
        this.items = response.items;
        this.totalCount = response.meta.totalCount;
      },
      error: () => this.showSnackBar(`Failed to load ${this.entityName}s`)
    });
  }

  private loadEmployees(): void {
    this.employeeService.getAll(1, 50).subscribe({
      next: (response) => {
        this.employees = response.items;
      }
    });
  }
}
