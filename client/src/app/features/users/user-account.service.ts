import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import { ApiResponse } from '../../core/models/api-response.model';

export interface CreateUserAccountPayload {
  employeeId: number;
  username: string;
  password: string;
  role: number;
}

export interface UserAccount {
  id: number;
  employeeId: number;
  employeeName: string;
  username: string;
  roleId: number;
  role: string;
  isProtected: boolean;
}

@Injectable({
  providedIn: 'root',
})
export class UserAccountService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl = `${environment.apiUrl}/auth`;

  create(payload: CreateUserAccountPayload): Observable<void> {
    return this.http.post<void>(
      `${this.apiUrl}/register`,
      payload
    );
  }

  getAll(): Observable<ApiResponse<UserAccount[]>> {
    return this.http.get<ApiResponse<UserAccount[]>>(
      `${this.apiUrl}/users`
    );
  }

  updateRole(id: number, role: number): Observable<ApiResponse<object>> {
    return this.http.put<ApiResponse<object>>(
      `${this.apiUrl}/users/${id}/role`,
      { role }
    );
  }

  delete(id: number): Observable<ApiResponse<object>> {
    return this.http.delete<ApiResponse<object>>(
      `${this.apiUrl}/users/${id}`
    );
  }
}
