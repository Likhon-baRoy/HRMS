import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { BaseListComponent } from '../../../core/base/base-list.component';
import { Employee } from '../employee.model';
import { EmployeeService } from '../employee.service';
import { EmployeeForm } from '../employee-form/employee-form';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatDialogModule
  ],
  templateUrl: './employee-list.html',
  styleUrl: './employee-list.scss'
})
export class EmployeeList extends BaseListComponent<Employee> {
  protected service = inject(EmployeeService);
  protected formComponent = EmployeeForm;
  protected entityName = 'Employee';

  displayedColumns: string[] = ['employeeCode', 'name', 'email', 'phone', 'department', 'position', 'employmentType', 'employeeStatus', 'actions' ];
}
