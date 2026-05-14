import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/employee`;

  getAll(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }

  create(data: any): Observable<any> {
    return this.http.post<any>(
      this.apiUrl,
      data
    );
  }

  update(
    id: number,
    data: any
  ): Observable<any> {
    return this.http.put<any>(
      `${this.apiUrl}/${id}`,
      data
    );
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(
      `${this.apiUrl}/${id}`
    );
  }
}
