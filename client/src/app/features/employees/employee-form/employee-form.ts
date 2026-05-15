import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { BaseFormComponent } from '../../../core/base/base-form.component';
import { EmployeeService } from '../employee.service';
import { DepartmentService } from '../../../core/services/department.service';
import { PositionService } from '../../../core/services/position.service';

export enum EmploymentType {
  Permanent = 1, Contract = 2, Probation = 3, Intern = 4, PartTime = 5
}

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, MatDialogModule, MatButtonModule,
    MatInputModule, MatFormFieldModule, MatSelectModule, MatSnackBarModule
  ],
  templateUrl: './employee-form.html',
  styleUrl: './employee-form.scss'
})
export class EmployeeForm extends BaseFormComponent<any> implements OnInit {
  protected service = inject(EmployeeService);
  private departmentService = inject(DepartmentService);
  private positionService = inject(PositionService);

  protected entityName = 'Employee';

  // Dropdown arrays
  departments: any[] = [];
  positions: any[] = [];
  allPositions: any[] = []; // Step 1: Holds full master records from API

  employmentTypes = [
    { value: EmploymentType.Permanent, label: 'Permanent' },
    { value: EmploymentType.Contract, label: 'Contract' },
    { value: EmploymentType.Probation, label: 'Probation' },
    { value: EmploymentType.Intern, label: 'Intern' },
    { value: EmploymentType.PartTime, label: 'Part Time' }
  ];

  form = this.fb.group({
    employeeCode: ['', Validators.required],
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    phone: ['', Validators.required],
    address: [''],
    dateOfBirth: ['', Validators.required],
    hireDate: ['', Validators.required],
    departmentId: [1, Validators.required],
    positionId: [1, Validators.required],
    employmentType: [1, Validators.required],
    accountNumber: ['']
  });

  maxBirthDate = new Date(
    new Date()
      .setFullYear(
        new Date()
          .getFullYear() - 18
      )
  );

  override ngOnInit(): void {
    super.ngOnInit(); // Setup base values
    this.loadDropdowns();
    this.watchDepartmentChange(); // Step 3: Start observing changes
  }

  // Step 5: Override data population to handle dynamic subset arrays on Edit
  protected override initializeForm(): void {
    if (this.data) {
      this.form.patchValue({
        ...this.data,
        employmentType: this.data.employmentTypeId,
        dateOfBirth: this.data.dateOfBirth?.split('T')[0],
        hireDate: this.data.hireDate?.split('T')[0]
      });

      // Ensure data is pre-filtered on historical edit payload mapping
      this.filterPositions(this.data.departmentId);
    }
  }

  private loadDropdowns(): void {
    this.departmentService.getAll().subscribe((response: any) => {
      this.departments = response.items;
    });

    this.positionService.getAll().subscribe((response: any) => {
      this.allPositions = response.items; // Step 2: Push items to master master lookup variable

      // If we are editing, apply initial filter immediately after master array loads
      if (this.data) {
        this.filterPositions(this.data.departmentId);
      }
    });
  }

  // Step 3: Reactive change stream pipeline
  private watchDepartmentChange(): void {
    this.form.get('departmentId')?.valueChanges.subscribe((departmentId) => {
      // Clear out chosen value on switch to avoid validation stale errors
      this.form.get('positionId')?.setValue(null);
      this.filterPositions(Number(departmentId));
    });
  }

  // Step 4: Conditional structural logic processor
  filterPositions(departmentId: number | null): void {
    if (!departmentId) {
      this.positions = [];
      return;
    }

    this.positions = this.allPositions.filter(
      (x) => x.departmentId === departmentId
    );
  }
}
