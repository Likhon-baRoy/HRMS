import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { BaseFormComponent } from '../../../core/base/base-form.component';
import { PositionService } from '../../../core/services/position.service';
import { DepartmentService } from '../../../core/services/department.service';

@Component({
  selector: 'app-position-form',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, MatDialogModule, MatButtonModule,
    MatInputModule, MatFormFieldModule, MatSelectModule, MatSnackBarModule
  ],
  templateUrl: './position-form.html',
  styleUrl: './position-form.scss'
})
export class PositionForm extends BaseFormComponent<any> implements OnInit {
  protected service = inject(PositionService);
  private departmentService = inject(DepartmentService);

  protected entityName = 'Position';
  departments: any[] = [];
  jobLevels = ['Intern', 'Junior', 'Mid', 'Senior', 'Lead', 'Manager'];

  form = this.fb.group({
    title: ['', Validators.required],
    jobLevel: ['', Validators.required],
    departmentId: [null, Validators.required]
  });

  override ngOnInit(): void {
    super.ngOnInit(); // Instantly handles the edit patchValue mapping behind the scenes
    this.loadDepartments();
  }

  private loadDepartments(): void {
    this.departmentService.getAll().subscribe({
      next: (response: any) => this.departments = response.items
    });
  }
}
