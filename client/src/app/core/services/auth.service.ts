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
      })
    );
  }

  logout(): void
  {
    localStorage
      .removeItem('token');

    this.router
      .navigate(['/login']);
  }

  getToken(): string | null
  {
    return localStorage
      .getItem('token');
  }

  isLoggedIn(): boolean
  {
    return !!this
      .getToken();
  }
}
