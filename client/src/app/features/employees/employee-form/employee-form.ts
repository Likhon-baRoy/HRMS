import {
  Component,
  Inject,
  inject
} from '@angular/core';

import { CommonModule } from '@angular/common';

import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  MAT_DIALOG_DATA,
  MatDialogModule,
  MatDialogRef
} from '@angular/material/dialog';

import { MatButtonModule } from '@angular/material/button';

import { MatFormFieldModule } from '@angular/material/form-field';

import { MatInputModule } from '@angular/material/input';

import { MatSelectModule } from '@angular/material/select';

import {
  MatSnackBar,
  MatSnackBarModule
} from '@angular/material/snack-bar';

import { EmployeeService } from '../employee.service';
import { DepartmentService } from '../../../core/services/department.service';
import { PositionService } from '../../../core/services/position.service';

export enum EmploymentType {
  Permanent = 1,
  Contract = 2,
  Probation = 3,
  Intern = 4,
  PartTime = 5
}

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatSelectModule,
    MatSnackBarModule
  ],
  templateUrl: './employee-form.html',
  styleUrl: './employee-form.scss'
})
export class EmployeeForm {
  private fb = inject(FormBuilder);

  private service = inject(EmployeeService);

  private snackBar = inject(MatSnackBar);

  private departmentService = inject(DepartmentService);

  private positionService = inject(PositionService);

  departments: any[] = [];
  positions: any[] = [];

  employmentTypes = [
    {
      value: EmploymentType.Permanent,
      label: 'Permanent'
    },
    {
      value: EmploymentType.Contract,
      label: 'Contract'
    },
    {
      value: EmploymentType.Probation,
      label: 'Probation'
    },
    {
      value: EmploymentType.Intern,
      label: 'Intern'
    },
    {
      value: EmploymentType.PartTime,
      label: 'Part Time'
    }
  ];

  form = this.fb.group({
    employeeCode: ['', Validators.required],

    firstName: ['', Validators.required],

    lastName: ['', Validators.required],

    email:
      [
        '',
        [
          Validators.required,
          Validators.email
        ]
      ],

    phone: ['', Validators.required],

    address: [''],

    dateOfBirth: ['', Validators.required],

    hireDate: ['', Validators.required],

    departmentId: [1, Validators.required],

    positionId: [1, Validators.required],

    employmentType:
      [
        1,
        Validators.required
      ],

    accountNumber: ['']
  });

  constructor(
    private dialogRef: MatDialogRef<EmployeeForm>,

    @Inject(MAT_DIALOG_DATA)
    public data: any
  ) {
    // LOAD DROPDOWNS
    this.loadDropdowns();

    // EDIT MODE
    if (data) {
      this.form.patchValue({
        ...data,

        employmentType: data.employmentTypeId,

        dateOfBirth: data.dateOfBirth?.split('T')[0],

        hireDate: data.hireDate?.split('T')[0]
      });
    }
  }

  loadDropdowns(): void {
    this.departmentService
      .getAll()
      .subscribe((response: any) => {
        this.departments = response.items;
      });

    this.positionService
      .getAll()
      .subscribe((response: any) => {
        this.positions = response.items;
      });
  }

  save(): void {
    if (this.form.invalid) {
      return;
    }

    const request = this.data
      ? this.service.update(
        this.data.id,
        this.form.value
      )
      : this.service.create(
        this.form.value
      );

    request.subscribe({
      next: () => {
        this.snackBar.open(
          this.data ? 'Employee updated' : 'Employee created',
          'Close',
          {
            duration: 3000
          }
        );

        this.dialogRef.close(true);
      },

      error: err => {
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
            duration: 3000
          }
        );
      }
    });
  }
}
