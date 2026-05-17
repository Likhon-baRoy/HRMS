import { Component, OnInit, inject } from '@angular/core';
import { ComponentType } from '@angular/cdk/portal';
import { MatDialog } from '@angular/material/dialog';
import { PageEvent } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import { getApiError } from '../utils/error-handler.util';
import { PagedResult } from '../models/paged-result.model';

// We define an interface for what a generic CRUD service must look like
interface CrudService<T> {
  getAll(page?: number, pageSize?: number): Observable<PagedResult<T>>;
  delete(id: number): Observable<any>;
}

@Component({ template: '' }) // Empty template required for Angular inheritance
export abstract class BaseListComponent<T> implements OnInit {
  protected abstract service: CrudService<T>;
  protected abstract formComponent: ComponentType<any>;
  protected abstract entityName: string; // e.g., 'Employee', 'Department'
  protected dialogWidth = '600px';
  protected pageSizeOptions = [5, 10, 25];

  // These are shared properties
  abstract displayedColumns: string[];
  items: T[] = [];
  pageIndex = 0;
  pageSize = 10;
  totalCount = 0;

  // Centralized injections
  protected dialog = inject(MatDialog);
  protected snackBar = inject(MatSnackBar);

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems(): void {
    this.service.getAll(this.pageIndex + 1, this.pageSize).subscribe({
      next: (response) => {
        this.items = response.items;
        this.totalCount = response.meta.totalCount;
      },
      error: (err) => this.showSnackBar(getApiError(err, `Failed to load ${this.entityName}s`))
    });
  }

  onPageChange(event: PageEvent): void {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadItems();
  }

  protected openForm(data?: T): void {
    const dialogRef = this.dialog.open(this.formComponent, {
      data: data,
      width: this.dialogWidth,
      maxWidth: '95vw'
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
