import { Component, inject, OnInit } from '@angular/core';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { DepartmentService } from '../../../core/services/department.service';
import { Department } from '../../../core/models/department.model';
import { DepartmentForm } from '../department-form/department-form';

@Component({
  selector: 'app-department-list',

  standalone: true,

  imports: [
    MatTableModule,
    MatCardModule,
    MatButtonModule,
    MatDialogModule
  ],

  templateUrl: './department-list.html',

  styleUrl: './department-list.scss'
})
export class DepartmentList
  implements OnInit {

  private service = inject(DepartmentService);

  private dialog = inject(MatDialog);

  displayedColumns = [
    'id',
    'name',
    'managerName',
    'actions'
  ];

  departments: Department[] = [];

  ngOnInit(): void {
    this.loadDepartments();
  }

  loadDepartments(): void {
    this.service
      .getAll()
      .subscribe({
        next:
          response => {
            this.departments =
              response.items;
          }
      });
  }

  create(): void {
    const dialog =
      this.dialog.open(
        DepartmentForm
      );

    dialog
      .afterClosed()
      .subscribe(result => {
        if (!result)
          return;

        this.service
          .create(result)
          .subscribe(() => {
            this.loadDepartments();
          });
      });
  }

  edit(department: Department): void {
    const dialog =
      this.dialog.open(
        DepartmentForm,
        {
          data:
            department
        }
      );

    dialog
      .afterClosed()
      .subscribe(result => {
        if (!result)
          return;

        this.service
          .update(
            department.id,
            result
          )
          .subscribe(() => {
            this.loadDepartments();
          });
      });
  }

  delete(id: number): void {

    if (!confirm('Delete department?')) {
      return;
    }

    this.service
      .delete(id)
      .subscribe(() => {
        this.loadDepartments();
      });
  }
}
