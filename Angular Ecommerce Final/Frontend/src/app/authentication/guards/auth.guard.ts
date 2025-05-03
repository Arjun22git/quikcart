import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { LoginComponent } from '../components/login/login.component';

export const authGuard: CanActivateFn = (route, state) => {
  const Login = inject(LoginComponent);
  const router = inject(Router);

  if (Login.isAuthenticated()) {
    router.navigateByUrl('/homepage')
    return true;
  }
  router.navigateByUrl('login');
  return false;

};
