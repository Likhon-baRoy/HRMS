import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatPaginatorModule } from '@angular/material/paginator';

import { BaseListComponent } from '../../../core/base/base-list.component';
import { Position } from '../../../core/models/position.model';
import { PositionService } from '../../../core/services/position.service';
import { PositionForm } from '../position-form/position-form';

@Component({
  selector: 'app-position-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatSnackBarModule,
    MatPaginatorModule
  ],
  templateUrl: './position-list.html',
  styleUrl: './position-list.scss'
})
export class PositionList extends BaseListComponent<Position> {
  // Tell the base component which service, form, and name to use
  protected service = inject(PositionService);
  protected formComponent = PositionForm;
  protected entityName = 'Position';

  // Setup your columns (the base class binds to 'items' automatically)
  displayedColumns: string[] = ['title', 'jobLevel', 'department', 'actions'];
}
