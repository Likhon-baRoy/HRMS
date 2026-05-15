import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { PositionService } from '../../../core/services/position.service';
import { Position } from '../../../core/models/position.model';
import { PositionForm } from '../position-form/position-form';
import { getApiError } from '../../../core/utils/error-handler.util';

@Component({
  selector: 'app-position-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatSnackBarModule
  ],
  templateUrl: './position-list.html',
  styleUrl: './position-list.scss'
})
export class PositionList implements OnInit {
  private readonly service = inject(PositionService);
  private readonly dialog = inject(MatDialog);
  private readonly snackBar = inject(MatSnackBar);

  displayedColumns: string[] = ['title', 'jobLevel', 'department', 'actions'];
  positions: Position[] = [];

  ngOnInit(): void {
    this.loadPositions();
  }

  loadPositions(): void {
    this.service.getAll().subscribe({
      next: (response) => this.positions = response.items,
      error: (err) => this.showSnackBar(getApiError(err, 'Failed to load positions'))
    });
  }

  private openForm(position?: Position): void {
    const dialogRef = this.dialog.open(PositionForm, {
      data: position,
      width: '600px'
    });

    dialogRef.afterClosed().subscribe((isSaved) => {
      if (isSaved) {
        this.loadPositions();
      }
    });
  }

  create(): void {
    this.openForm();
  }

  edit(position: Position): void {
    this.openForm(position);
  }

  delete(id: number): void {
    if (!confirm('Are you sure you want to delete this position?')) {
      return;
    }

    this.service.delete(id).subscribe({
      next: () => {
        this.showSnackBar('Position deleted successfully');
        this.loadPositions();
      },
      error: (err) => this.showSnackBar(getApiError(err, 'Delete failed'))
    });
  }

  private showSnackBar(message: string): void {
    this.snackBar.open(message, 'Close', { duration: 3000 });
  }
}
