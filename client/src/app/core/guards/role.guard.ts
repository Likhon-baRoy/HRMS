import { inject } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const roleGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  if (!auth.isLoggedIn()) {
    router.navigate(['/login']);
    return false;
  }

  const allowedRoles = route.data['roles'] as string[] | undefined;
  const role = auth.getRole() ?? '';

  if (!role) {
    auth.logout();
    return false;
  }

  if (!allowedRoles || allowedRoles.includes(role)) {
    return true;
  }

  const fallback = role === 'Employee' ? '/attendance' : '/dashboard';

  if (router.url !== fallback) {
    router.navigate([fallback]);
  }

  return false;
};
