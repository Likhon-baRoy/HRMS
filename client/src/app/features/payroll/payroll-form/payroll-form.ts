import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { GeneratePayrollPayload, PayrollService } from '../payroll.service';

@Component({
  selector: 'app-payroll-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule,
  ],
  templateUrl: './payroll-form.html',
  styleUrl: './payroll-form.scss',
})
export class PayrollForm {
  private readonly fb = inject(FormBuilder);
  private readonly service = inject(PayrollService);
  private readonly dialogRef = inject(MatDialogRef<PayrollForm>);
  private readonly snackBar = inject(MatSnackBar);

  form = this.fb.group({
    payPeriodStart: ['', Validators.required],
    payPeriodEnd: ['', Validators.required],
    bonusAmount: [0, [Validators.required, Validators.min(0)]],
    deductionAmount: [0, [Validators.required, Validators.min(0)]],
    taxPercent: [0, [Validators.required, Validators.min(0), Validators.max(100)]],
  });

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const raw = this.form.getRawValue();

    const payload: GeneratePayrollPayload = {
      payPeriodStart: raw.payPeriodStart as string,
      payPeriodEnd: raw.payPeriodEnd as string,
      bonusAmount: Number(raw.bonusAmount ?? 0),
      deductionAmount: Number(raw.deductionAmount ?? 0),
      taxPercent: Number(raw.taxPercent ?? 0),
    };

    this.service.generate(payload).subscribe({
      next: () => {
        this.snackBar.open('Payroll generated successfully', 'Close', { duration: 3000 });
        this.dialogRef.close(true);
      },
      error: () => this.snackBar.open('Operation failed', 'Close', { duration: 3000 }),
    });
  }
}
