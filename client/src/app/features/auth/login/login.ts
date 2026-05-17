import {
  Component,
  inject
} from '@angular/core';

import {
  Router
} from '@angular/router';

import {
  FormsModule
} from '@angular/forms';

import {
  MatCardModule
} from '@angular/material/card';

import {
  MatFormFieldModule
} from '@angular/material/form-field';

import {
  MatInputModule
} from '@angular/material/input';

import {
  MatButtonModule
} from '@angular/material/button';

import {
  AuthService
} from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',

  standalone: true,

  imports: [
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],

  templateUrl: './login.html',

  styleUrl: './login.scss'
})

export class Login {
  username = '';

  password = '';

  private auth = inject( AuthService );

  private router = inject( Router );

  login(): void {
    this.auth
      .login({
        username: this.username,
        password: this.password
      })
      .subscribe({
        next: () => {
          const role = this.auth.getRole();

          this.router
            .navigate([
              role === 'Employee'
                ? '/attendance'
                : '/dashboard'
            ]);
        },
        error: console.error
      });
  }
}
