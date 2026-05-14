import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-department-form',

  standalone: true,

  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],

  templateUrl: './department-form.html'
})
export class DepartmentForm {

  form!: FormGroup;

  constructor(
    private fb: FormBuilder,

    private dialogRef: MatDialogRef<DepartmentForm>,

    @Inject(MAT_DIALOG_DATA)
    public data: any
  ) {
    this.form =
      this.fb.group({
        name: [
          this.data?.name ?? '',
          Validators.required
        ],

        description: [
          this.data?.description ?? ''
        ]
      });
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.dialogRef.close(
      this.form.value
    );
  }
}
