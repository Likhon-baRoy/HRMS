import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { PagedResult } from '../models/paged-result.model';
import { Attendance } from '../models/attendance.model';

@Injectable({
  providedIn: 'root',
})
export class AttendanceService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    `${environment.apiUrl}/attendance`;

  getAll(page = 1, pageSize = 10, filters?: {
    employeeId?: number | null;
    attendanceDate?: string | null;
  }): Observable<PagedResult<Attendance>> {
    const params = new URLSearchParams({
      page: String(page),
      pageSize: String(pageSize)
    });

    if (filters?.employeeId) {
      params.set('employeeId', String(filters.employeeId));
    }

    if (filters?.attendanceDate) {
      params.set('attendanceDate', filters.attendanceDate);
    }

    return this.http.get<PagedResult<Attendance>>(
      `${this.apiUrl}?${params.toString()}`
    );
  }

  checkIn(
    remarks?: string
  ): Observable<any> {
    return this.http.post(
      `${this.apiUrl}/check-in`,
      { remarks }
    );
  }

  checkOut(): Observable<any> {
    return this.http.post(
      `${this.apiUrl}/check-out`,
      {}
    );
  }
}
