import { Component, inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';

import { AuthService } from '../../../core/services/auth.service';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    MatToolbarModule,
    MatButtonModule
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class Dashboard implements OnInit {
  private auth = inject(AuthService);
  private http = inject(HttpClient);

  stats: any = null;

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.http
      .get(`${environment.apiUrl}/dashboard`)
      .subscribe({
        next: (response) => {
          console.log(response);
          this.stats = response;
        },

        error: console.error
      });
  }

  logout(): void {
    this.auth.logout();
  }
}
