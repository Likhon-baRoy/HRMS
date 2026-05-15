import { Component, OnInit, inject } from '@angular/core';
import { ComponentType } from '@angular/cdk/portal';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import { getApiError } from '../utils/error-handler.util';

// We define an interface for what a generic CRUD service must look like
interface CrudService<T> {
  getAll(): Observable<{ items: T[] }>;
  delete(id: number): Observable<any>;
}

@Component({ template: '' }) // Empty template required for Angular inheritance
export abstract class BaseListComponent<T> implements OnInit {
  protected abstract service: CrudService<T>;
  protected abstract formComponent: ComponentType<any>;
  protected abstract entityName: string; // e.g., 'Employee', 'Department'

  // These are shared properties
  abstract displayedColumns: string[];
  items: T[] = [];

  // Centralized injections
  protected dialog = inject(MatDialog);
  protected snackBar = inject(MatSnackBar);

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems(): void {
    this.service.getAll().subscribe({
      next: (response) => this.items = response.items,
      error: (err) => this.showSnackBar(getApiError(err, `Failed to load ${this.entityName}s`))
    });
  }

  private openForm(data?: T): void {
    const dialogRef = this.dialog.open(this.formComponent, {
      data: data,
      width: '600px'
    });

    dialogRef.afterClosed().subscribe((isSaved) => {
      if (isSaved) {
        this.loadItems();
      }
    });
  }

  create(): void {
    this.openForm();
  }

  edit(item: T): void {
    this.openForm(item);
  }

  delete(id: number): void {
    if (!confirm(`Are you sure you want to delete this ${this.entityName.toLowerCase()}?`)) {
      return;
    }

    this.service.delete(id).subscribe({
      next: () => {
        this.showSnackBar(`${this.entityName} deleted successfully`);
        this.loadItems();
      },
      error: (err) => this.showSnackBar(getApiError(err, 'Delete failed'))
    });
  }

  protected showSnackBar(message: string): void {
    this.snackBar.open(message, 'Close', { duration: 3000 });
  }
}
