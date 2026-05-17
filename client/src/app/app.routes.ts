import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

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
        path: '',
        redirectTo: 'attendance',
        pathMatch: 'full'
      },

      {
        path: 'dashboard',
        canActivate: [roleGuard],
        data: { roles: ['Admin', 'HR', 'Manager'] },
        loadComponent: () =>
          import('./features/dashboard/dashboard/dashboard')
            .then(m => m.Dashboard)
      },

      {
        path: 'employees',
        canActivate: [roleGuard],
        data: { roles: ['Admin', 'HR', 'Manager'] },
        loadComponent: () =>
          import('./features/employees/employee-list/employee-list')
            .then(m => m.EmployeeList)
      },

      {
        path: 'departments',
        canActivate: [roleGuard],
        data: { roles: ['Admin', 'HR'] },
        loadComponent: () =>
          import('./features/departments/department-list/department-list')
            .then(m => m.DepartmentList)
      },

      {
        path: 'positions',
        canActivate: [roleGuard],
        data: { roles: ['Admin', 'HR'] },
        loadComponent: () =>
          import('./features/positions/position-list/position-list')
            .then(m => m.PositionList)
      },

      {
        path: 'salary',
        canActivate: [roleGuard],
        data: { roles: ['Admin', 'HR'] },
        loadComponent: () =>
          import('./features/salary/salary-list/salary-list')
            .then(m => m.SalaryList)
      },

      {
        path: 'payroll',
        canActivate: [roleGuard],
        data: { roles: ['Admin', 'HR'] },
        loadComponent: () =>
          import('./features/payroll/payroll-list/payroll-list')
            .then(m => m.PayrollList)
      },

      {
        path: 'attendance',
        canActivate: [roleGuard],
        data: { roles: ['Admin', 'HR', 'Manager', 'Employee'] },
        loadComponent: () =>
          import('./features/attendance/attendance-list/attendance-list')
            .then(m => m.AttendanceList)
      }
    ]
  },

  {
    path: '**',
    redirectTo: 'login'
  }
];
