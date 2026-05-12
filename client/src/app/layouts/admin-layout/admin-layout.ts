import { Component, inject } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';

import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [
    RouterOutlet,
    RouterLink,

    MatSidenavModule,
    MatToolbarModule,
    MatListModule,
    MatButtonModule,
    MatIconModule
  ],
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.scss'
})
export class AdminLayout {
  private auth = inject(AuthService);

  logout(): void {
    this.auth.logout();
  }
}
