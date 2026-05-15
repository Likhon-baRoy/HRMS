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

  getAll():
    Observable<PagedResult<Employee>> {
    return this.http.get<any>(this.apiUrl);
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
