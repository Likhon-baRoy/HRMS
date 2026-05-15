import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { DepartmentService } from '../../../core/services/department.service';
import { Department } from '../../../core/models/department.model';
import { DepartmentForm } from '../department-form/department-form';
import { getApiError } from '../../../core/utils/error-handler.util';

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
    MatDialogModule
  ],
  templateUrl: './department-list.html',
  styleUrl: './department-list.scss'
})
export class DepartmentList implements OnInit {
  private readonly departmentService = inject(DepartmentService);
  private readonly snackBar = inject(MatSnackBar);
  private readonly dialog = inject(MatDialog);

  displayedColumns: string[] = ['id', 'name', 'managerName', 'actions'];
  departments: Department[] = [];

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.departmentService.getAll().subscribe({
      next: (response) => this.departments = response.items,
      error: (err) => this.showSnackBar(getApiError(err, 'Failed to load Departments'))
    });
  }

  private openForm(department?: Department): void {
    const dialogRef = this.dialog.open(DepartmentForm, {
      data: department,
      width: '700px'
    });

    dialogRef.afterClosed().subscribe((isSaved) => {
      if (isSaved) this.loadDepartments();
    });
  }

  create(): void {
    this.openForm();
  }

  edit(department: Department): void {
    this.openForm(department);
  }

  delete(id: number): void {
    if (!confirm('Are you sure you want to delete this department?')) {
      return;
    }

    this.departmentService.delete(id).subscribe({
      next: () => {
        this.showSnackBar('Department deleted successfully');
        this.loadDepartments();
      },
      error: (err) => this.showSnackBar(getApiError(err, 'Delete failed'))
    });
  }

  private showSnackBar(message: string): void {
    this.snackBar.open(message, 'Close', { duration: 3000 });
  }
}
