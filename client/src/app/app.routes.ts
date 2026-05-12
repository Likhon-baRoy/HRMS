import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },

  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/login/login')
        .then(m => m.Login)
  },

  {
    path: '',
    canActivate: [authGuard],

    loadComponent: () =>
      import('./layouts/admin-layout/admin-layout')
        .then(m => m.AdminLayout),

    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import(
            './features/dashboard/dashboard/dashboard'
          ).then(m => m.Dashboard)
      }
    ]
  },

  {
    path: '**',
    redirectTo: 'login'
  }
];
