import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../core/models/api-response.model';
import { PagedResult } from '../../core/models/paged-result.model';
import { Payroll } from './payroll.model';

export interface GeneratePayrollPayload {
  payPeriodStart: string;
  payPeriodEnd: string;
}

@Injectable({
  providedIn: 'root',
})
export class PayrollService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/payroll`;

  getAll(page = 1, pageSize = 10): Observable<PagedResult<Payroll>> {
    return this.http.get<PagedResult<Payroll>>(
      `${this.apiUrl}?page=${page}&pageSize=${pageSize}`
    );
  }

  generate(payload: GeneratePayrollPayload): Observable<ApiResponse<object>> {
    return this.http.post<ApiResponse<object>>(this.apiUrl, payload);
  }
}
