import { Component, inject, OnInit } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { MatCardModule } from '@angular/material/card';

import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    MatCardModule
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class Dashboard
  implements OnInit {

  private http =
    inject(HttpClient);

  stats: any = null;

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.http
      .get(
        `${environment.apiUrl}/dashboard`
      )
      .subscribe({
        next: (
          response
        ) => {
          this.stats =
            response;
        },

        error:
          console.error
      });
  }
}
