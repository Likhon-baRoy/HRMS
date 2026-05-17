import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';
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
    MatPaginatorModule,
  ],
  templateUrl: './payroll-list.html',
  styleUrl: './payroll-list.scss',
})
export class PayrollList {
  private readonly service = inject(PayrollService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  items: Payroll[] = [];
  pageIndex = 0;
  pageSize = 10;
  totalCount = 0;
  readonly pageSizeOptions = [5, 10, 25];

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

  get employeeCount(): number {
    return this.items.length;
  }

  get grossTotal(): number {
    return this.sum('grossSalary');
  }

  get deductionTotal(): number {
    return this.sum('totalDeductions');
  }

  get netTotal(): number {
    return this.sum('netSalary');
  }

  loadItems(): void {
    this.service.getAll(this.pageIndex + 1, this.pageSize).subscribe({
      next: (response) => {
        this.items = response.items;
        this.totalCount = response.meta.totalCount;
      },
      error: () => this.snackBar.open('Failed to load payroll', 'Close', { duration: 3000 }),
    });
  }

  onPageChange(event: { pageIndex: number; pageSize: number }): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadItems();
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

  private sum(key: keyof Payroll): number {
    return this.items.reduce(
      (total, item) => total + Number(item[key] ?? 0),
      0
    );
  }
}
