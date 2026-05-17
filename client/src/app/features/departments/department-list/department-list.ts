import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatPaginatorModule } from '@angular/material/paginator';

import { BaseListComponent } from '../../../core/base/base-list.component';
import { Department } from '../../../core/models/department.model';
import { DepartmentService } from '../../../core/services/department.service';
import { DepartmentForm } from '../department-form/department-form';

@Component({
  selector: 'app-department-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatDialogModule,
    MatPaginatorModule
  ],
  templateUrl: './department-list.html',
  styleUrl: './department-list.scss'
})
export class DepartmentList extends BaseListComponent<Department> {
  protected service = inject(DepartmentService);
  protected formComponent = DepartmentForm;
  protected entityName = 'Department';

  displayedColumns: string[] = ['id', 'name', 'managerName', 'actions'];
}
