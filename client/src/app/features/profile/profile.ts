import { CommonModule } from '@angular/common';
import { Component, OnInit, inject } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { UserProfile } from '../../core/models/user-profile.model';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './profile.html',
  styleUrl: './profile.scss'
})
export class Profile implements OnInit {
  private readonly authService = inject(AuthService);

  profile?: UserProfile;

  isLoading = true;

  errorMessage = '';

  ngOnInit(): void {
    this.authService.getProfile().subscribe({
      next: (response) => {
        this.profile = response.data;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Failed to load profile';
        this.isLoading = false;
      }
    });
  }
}
