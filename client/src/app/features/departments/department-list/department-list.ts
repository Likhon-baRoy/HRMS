import { Component, inject, OnInit } from '@angular/core';

import { MatTableModule } from '@angular/material/table';

import { MatCardModule } from '@angular/material/card';

import { DepartmentService } from '../../../core/services/department.service';

import { Department } from '../../../core/models/department.model';

@Component({
  selector: 'app-department-list',

  standalone: true,

  imports: [
    MatTableModule,
    MatCardModule
  ],

  templateUrl: './department-list.html',

  styleUrl: './department-list.scss'
})

export class DepartmentList
  implements OnInit {

  private service = inject(DepartmentService);

  displayedColumns =
    [
      'id',
      'name',
      'managerName'
    ];

  departments: Department[] = [];

  ngOnInit():
    void {
    this.loadDepartments();
  }

  loadDepartments():
    void {
    this.service
      .getAll()
      .subscribe({
        next:
          response => {
            this.departments =
              response.items;
          },

        error:
          console.error
      });
  }
}
