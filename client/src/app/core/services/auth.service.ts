import { inject, Injectable }
from '@angular/core';

import {
  HttpClient
} from '@angular/common/http';

import {
  Router
} from '@angular/router';

import {
  Observable,
  tap
} from 'rxjs';

import {
  environment
} from '../../../environments/environment';

import {
  LoginRequest
} from '../models/login-request.model';

import {
  ApiResponse
} from '../models/api-response.model';

import {
  AuthResponse
} from '../models/auth-response.model';

import {
  UserProfile
} from '../models/user-profile.model';

@Injectable({
  providedIn: 'root'
})

export class AuthService
{
  private http = inject(HttpClient);

  private router = inject(Router);

  login(payload: LoginRequest): Observable<ApiResponse<AuthResponse>>
  {
    return this.http.post<ApiResponse<AuthResponse>>(
      `${environment.apiUrl}/auth/login`,
      payload
    )
    .pipe(
      tap(response =>
      {
        localStorage
          .setItem(
            'token',
            response.data.token
          );

        localStorage
          .setItem(
            'role',
            response.data.role
          );

        localStorage
          .setItem(
            'roleId',
            String(response.data.roleId)
          );

        localStorage
          .setItem(
            'username',
            response.data.username
          );
      })
    );
  }

  logout(): void
  {
    localStorage
      .removeItem('token');

    localStorage
      .removeItem('role');

    localStorage
      .removeItem('roleId');

    localStorage
      .removeItem('username');

    this.router
      .navigate(['/login']);
  }

  getToken(): string | null
  {
    return localStorage
      .getItem('token');
  }

  getProfile(): Observable<ApiResponse<UserProfile>>
  {
    return this.http.get<ApiResponse<UserProfile>>(
      `${environment.apiUrl}/auth/profile`
    );
  }

  isLoggedIn(): boolean
  {
    return !!this
      .getToken();
  }

  getRole(): string | null
  {
    const storedRole = localStorage
      .getItem('role');

    if (storedRole) {
      return storedRole;
    }

    const token = this.getToken();

    if (!token) {
      return null;
    }

    try {
      const payload = JSON.parse(
        atob(token.split('.')[1])
      );

      return payload.role
        ?? payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
        ?? null;
    } catch {
      return null;
    }
  }

  isEmployee(): boolean
  {
    return this.getRole() === 'Employee';
  }

  isAdmin(): boolean
  {
    return this.getRole() === 'Admin';
  }

  isHrOrAdmin(): boolean
  {
    return ['Admin', 'HR']
      .includes(this.getRole() ?? '');
  }

  isStaff(): boolean
  {
    return ['Admin', 'HR', 'Manager']
      .includes(this.getRole() ?? '');
  }
}
