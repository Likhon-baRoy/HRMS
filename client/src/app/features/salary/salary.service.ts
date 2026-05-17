import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../core/models/api-response.model';
import { PagedResult } from '../../core/models/paged-result.model';
import { Salary } from './salary.model';

export interface SalaryPayload {
  employeeId: number;
  basicSalary: number;
  houseRent: number;
  medicalAllowance: number;
  transportAllowance: number;
  otherAllowance: number;
  effectiveDate: string;
}

@Injectable({
  providedIn: 'root',
})
export class SalaryService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/salary`;

  getAll(page = 1, pageSize = 10, filters?: {
    employeeId?: number | null;
  }): Observable<PagedResult<Salary>> {
    const params = new URLSearchParams({
      page: String(page),
      pageSize: String(pageSize)
    });

    if (filters?.employeeId) {
      params.set('employeeId', String(filters.employeeId));
    }

    return this.http.get<PagedResult<Salary>>(
      `${this.apiUrl}?${params.toString()}`
    );
  }

  create(payload: SalaryPayload): Observable<ApiResponse<object>> {
    return this.http.post<ApiResponse<object>>(this.apiUrl, payload);
  }

  update(id: number, payload: SalaryPayload): Observable<ApiResponse<object>> {
    return this.http.put<ApiResponse<object>>(`${this.apiUrl}/${id}`, payload);
  }

  delete(id: number): Observable<ApiResponse<object>> {
    return this.http.delete<ApiResponse<object>>(`${this.apiUrl}/${id}`);
  }
}
