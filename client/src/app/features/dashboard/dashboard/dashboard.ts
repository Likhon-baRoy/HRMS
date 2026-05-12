import { Component, inject } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-dashboard',

  standalone: true,

  imports: [
    MatToolbarModule,
    MatButtonModule
  ],

  templateUrl: './dashboard.html',

  styleUrl: './dashboard.scss',
})

export class Dashboard
{
  private auth = inject(AuthService);

  logout(): void
  {
    this.auth.logout();
  }
}
