import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const roleGuard = (requiredRole: string): CanActivateFn => {
  return (_route, _state) => {
    const authService = inject(AuthService);
    const user = authService.currentUser();
    if (user?.roles.includes(requiredRole)) return true;
    return inject(Router).createUrlTree(['/']);
  };
};
