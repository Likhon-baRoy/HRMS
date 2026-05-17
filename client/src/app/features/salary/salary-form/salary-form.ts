import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { BaseFormComponent } from '../../../core/base/base-form.component';
import { Employee } from '../../employees/employee.model';
import { EmployeeService } from '../../employees/employee.service';
import { SalaryService, SalaryPayload } from '../salary.service';
import { Salary } from '../salary.model';

@Component({
  selector: 'app-salary-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatSnackBarModule,
  ],
  templateUrl: './salary-form.html',
  styleUrl: './salary-form.scss',
})
export class SalaryForm extends BaseFormComponent<Salary> implements OnInit {
  protected service = inject(SalaryService);
  private readonly employeeService = inject(EmployeeService);
  protected readonly entityName = 'Salary';

  employees: Employee[] = [];

  form = this.fb.group({
    employeeId: [null as number | null, Validators.required],
    basicSalary: [null as number | null, [Validators.required, Validators.min(0.01)]],
    houseRent: [0, [Validators.required, Validators.min(0)]],
    medicalAllowance: [0, [Validators.required, Validators.min(0)]],
    transportAllowance: [0, [Validators.required, Validators.min(0)]],
    otherAllowance: [0, [Validators.required, Validators.min(0)]],
    effectiveDate: ['', Validators.required],
  });

  override ngOnInit(): void {
    super.ngOnInit();
    this.loadEmployees();
  }

  protected override initializeForm(): void {
    if (!this.data) {
      return;
    }

    this.form.patchValue({
      ...this.data,
      effectiveDate: this.data.effectiveDate?.split('T')[0],
    });
  }

  override save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.getRawValue();

    const payload: SalaryPayload = {
      employeeId: Number(raw.employeeId),
      basicSalary: Number(raw.basicSalary),
      houseRent: Number(raw.houseRent ?? 0),
      medicalAllowance: Number(raw.medicalAllowance ?? 0),
      transportAllowance: Number(raw.transportAllowance ?? 0),
      otherAllowance: Number(raw.otherAllowance ?? 0),
      effectiveDate: raw.effectiveDate as string,
    };

    const request$ = this.data
      ? this.service.update(this.data.id, payload)
      : this.service.create(payload);

    request$.subscribe({
      next: () => {
        this.showSnackBar(
          `${this.entityName} ${this.data ? 'updated' : 'assigned'} successfully`
        );
        this.dialogRef.close(true);
      },
      error: () => this.showSnackBar('Operation failed'),
    });
  }

  private loadEmployees(): void {
    this.employeeService.getAll(1, 50).subscribe({
      next: (response) => {
        this.employees = response.items;
      },
    });
  }
}
