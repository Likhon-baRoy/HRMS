import { Component, OnInit, inject, Inject } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import { getApiError } from '../utils/error-handler.util';

interface FormService<T> {
  create(payload: any): Observable<any>;
  update(id: number, payload: any): Observable<any>;
}

@Component({ template: '' })
export abstract class BaseFormComponent<T> implements OnInit {
  // Shared structural dependencies
  protected fb = inject(FormBuilder);
  protected snackBar = inject(MatSnackBar);
  protected dialogRef = inject(MatDialogRef<any>);

  // Implicit context configuration injected by Angular Material Dialog
  protected data = inject(MAT_DIALOG_DATA, { optional: true });

  // Core targets defined explicitly by the extending child component
  protected abstract service: FormService<T>;
  protected abstract entityName: string; // e.g., 'Employee'
  abstract form: FormGroup;

  ngOnInit(): void {
    this.initializeForm();
  }

  protected initializeForm(): void {
    if (this.data) {
      // If data is provided, map data to matching input controls automatically
      this.form.patchValue(this.data);
    }
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    // Determine strategy pattern based on edit or create instance context
    const request$ = this.data
      ? this.service.update(this.data.id, this.form.value)
      : this.service.create(this.form.value);

    request$.subscribe({
      next: () => {
        this.showSnackBar(`${this.entityName} ${this.data ? 'updated' : 'created'} successfully`);
        this.dialogRef.close(true); // Return 'true' to signal the list layout to reload
      },
      error: (err) => this.showSnackBar(getApiError(err, 'Operation failed'))
    });
  }

  protected showSnackBar(message: string): void {
    this.snackBar.open(message, 'Close', { duration: 3000 });
  }
}
