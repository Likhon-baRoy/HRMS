import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { PagedResult } from '../../core/models/paged-result.model';
import { Employee } from './employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/employee`;

  getAll(page = 1, pageSize = 10, filters?: {
    search?: string;
    departmentId?: number | null;
    employeeStatus?: number | null;
  }):
    Observable<PagedResult<Employee>> {
    const params = new URLSearchParams({
      page: String(page),
      pageSize: String(pageSize)
    });

    if (filters?.search) {
      params.set('search', filters.search);
    }

    if (filters?.departmentId) {
      params.set('departmentId', String(filters.departmentId));
    }

    if (filters?.employeeStatus) {
      params.set('employeeStatus', String(filters.employeeStatus));
    }

    return this.http.get<PagedResult<Employee>>(
      `${this.apiUrl}?${params.toString()}`
    );
  }

  getById(id: number):
    Observable<any> {
    return this.http.get(
      `${this.apiUrl}/${id}`
    );
  }

  create(data: any):
    Observable<PagedResult<Employee>> {
    return this.http.post<any>(
      this.apiUrl,
      data
    );
  }

  update(id: number, data: any):
    Observable<PagedResult<Employee>> {
    return this.http.put<any>(
      `${this.apiUrl}/${id}`,
      data
    );
  }

  delete(id: number):
    Observable<any> {
    return this.http.delete<any>(
      `${this.apiUrl}/${id}`
    );
  }
}
