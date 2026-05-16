import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AttendanceService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    `${environment.apiUrl}/attendance`;

  getAll(): Observable<any> {
    return this.http.get(this.apiUrl);
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