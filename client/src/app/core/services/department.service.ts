import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Department } from '../models/department.model';
import { PagedResult } from '../models/paged-result.model';
import { ApiResponse } from '../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/department`;

  getAll(page = 1, pageSize = 10): Observable<PagedResult<Department>> {
    return this.http.get<PagedResult<Department>>(
      `${this.apiUrl}?page=${page}&pageSize=${pageSize}`
    );
  }

  create(
    payload: {
      name: string;
      description?: string;
      managerId?: number | null;
    }
  ): Observable<ApiResponse<object>> {
    return this.http.post<ApiResponse<object>>(this.apiUrl, payload);
  }

  update(
    id: number,
    payload: {
      name: string;
      description?: string;
      managerId?: number | null;
    }
  ): Observable<ApiResponse<object>> {
    return this.http.put<ApiResponse<object>>(`${this.apiUrl}/${id}`, payload);
  }

  delete(id: number): Observable<ApiResponse<object>> {
    return this.http.delete<ApiResponse<object>>(`${this.apiUrl}/${id}`);
  }
}
