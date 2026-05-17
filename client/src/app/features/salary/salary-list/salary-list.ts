import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';

import { BaseListComponent } from '../../../core/base/base-list.component';
import { Salary } from '../salary.model';
import { SalaryService } from '../salary.service';
import { SalaryForm } from '../salary-form/salary-form';

@Component({
  selector: 'app-salary-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatSnackBarModule,
  ],
  templateUrl: './salary-list.html',
  styleUrl: './salary-list.scss',
})
export class SalaryList extends BaseListComponent<Salary> {
  protected service = inject(SalaryService);
  protected formComponent = SalaryForm;
  protected entityName = 'Salary';

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
}
