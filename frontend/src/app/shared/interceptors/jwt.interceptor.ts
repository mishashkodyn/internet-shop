import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, catchError, filter, switchMap, take, throwError } from 'rxjs';
import { AuthService } from '../services/auth.service';

let isRefreshing = false;
const refreshSubject = new BehaviorSubject<string | null>(null);

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  // Do not intercept the refresh endpoint itself (prevents infinite recursion)
  const isRefreshCall = req.url.includes('/api/auth/refresh');
  const isApiCall = req.url.includes('/api/');

  const addBearer = (token: string) =>
    req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });

  if (!isApiCall || isRefreshCall) return next(req);

  const token = authService.getAccessToken();
  const outgoing = token ? addBearer(token) : req;

  return next(outgoing).pipe(
    catchError((err: unknown) => {
      if (!(err instanceof HttpErrorResponse) || err.status !== 401) return throwError(() => err);

      if (isRefreshing) {
        // Queue this request until the refresh completes
        return refreshSubject.pipe(
          filter((t): t is string => t !== null),
          take(1),
          switchMap(newToken => next(addBearer(newToken)))
        );
      }

      isRefreshing = true;
      refreshSubject.next(null);

      return authService.refreshTokens().pipe(
        switchMap(tokens => {
          isRefreshing = false;
          refreshSubject.next(tokens.accessToken);
          return next(addBearer(tokens.accessToken));
        }),
        catchError(refreshError => {
          isRefreshing = false;
          authService.clearSession();
          router.navigate(['/login']);
          return throwError(() => refreshError);
        })
      );
    })
  );
};
