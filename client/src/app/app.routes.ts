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
          import('./features/dashboard/dashboard/dashboard')
            .then(m => m.Dashboard)
      },

      {
        path: 'employees',
        loadComponent: () =>
          import('./features/employees/employee-list/employee-list')
            .then(m => m.EmployeeList)
      },

      {
        path: 'departments',
        loadComponent: () =>
          import('./features/departments/department-list/department-list')
            .then(m => m.DepartmentList)
      },

      {
        path: 'positions',
        loadComponent: () =>
          import('./features/positions/position-list/position-list')
            .then(m => m.PositionList)
      }
    ]
  },

  {
    path: '**',
    redirectTo: 'login'
  }
];
