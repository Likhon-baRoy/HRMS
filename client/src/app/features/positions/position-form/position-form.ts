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

import { PositionService } from '../../../core/services/position.service';
import { DepartmentService } from '../../../core/services/department.service';

@Component({
  selector: 'app-position-form',
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
  templateUrl: './position-form.html',
  styleUrl: './position-form.scss'
})
export class PositionForm {
  private fb =
    inject(FormBuilder);

  private service =
    inject(PositionService);

  private departmentService =
    inject(DepartmentService);

  private snackBar =
    inject(MatSnackBar);

  departments: any[] = [];

  jobLevels = ['Intern', 'Junior', 'Mid', 'Senior', 'Lead', 'Manager'];

  form = this.fb.group({
    title: [
      '',
      Validators.required
    ],

    jobLevel: [''],

    departmentId: [
      null,
      Validators.required
    ]
  });

  constructor(
    private dialogRef:
      MatDialogRef<PositionForm>,

    @Inject(MAT_DIALOG_DATA)
    public data: any
  ) {
    this.loadDepartments();

    if (data) {
      this.form.patchValue(
        data
      );
    }
  }

  loadDepartments(): void {
    this.departmentService
      .getAll()
      .subscribe(
        (response: any) => {
          this.departments =
            response.items;
        }
      );
  }

  save(): void {
    if (
      this.form.invalid
    ) {
      return;
    }

    const request =
      this.data
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
          this.data
            ? 'Position updated'
            : 'Position created',
          'Close',
          {
            duration: 3000
          }
        );

        this.dialogRef.close(
          true
        );
      },

      error: err => {
        let message =
          'Operation failed';

        const errors =
          err.error?.errors;

        if (errors) {
          const firstKey =
            Object.keys(errors)[0];

          if (firstKey) {
            message =
              errors[
              firstKey
              ][0];
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
