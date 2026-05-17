import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';

import { Payroll } from '../payroll.model';
import { PayrollService } from '../payroll.service';
import { PayrollForm } from '../payroll-form/payroll-form';

@Component({
  selector: 'app-payroll-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatDialogModule,
    MatIconModule,
    MatSnackBarModule,
  ],
  templateUrl: './payroll-list.html',
  styleUrl: './payroll-list.scss',
})
export class PayrollList {
  private readonly service = inject(PayrollService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  items: Payroll[] = [];

  displayedColumns: string[] = [
    'employee',
    'period',
    'gross',
    'bonus',
    'deductions',
    'tax',
    'net',
    'status',
    'generatedAt',
  ];

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems(): void {
    this.service.getAll().subscribe({
      next: (response) => this.items = response.items,
      error: () => this.snackBar.open('Failed to load payroll', 'Close', { duration: 3000 }),
    });
  }

  generate(): void {
    const dialogRef = this.dialog.open(PayrollForm, {
      width: '480px'
    });

    dialogRef.afterClosed().subscribe((saved) => {
      if (saved) {
        this.loadItems();
      }
    });
  }
}
