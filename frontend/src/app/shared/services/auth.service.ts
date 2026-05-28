import { Injectable, computed, signal, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { LoginRequest, RegisterRequest, TokenPair, UserInfo } from '../models/auth.models';

// TODO migrate to httpOnly cookies
const ACCESS_TOKEN_KEY = 'access_token';
const REFRESH_TOKEN_KEY = 'refresh_token';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);

  readonly currentUser = signal<UserInfo | null>(null);
  readonly isAuthenticated = computed(() => this.currentUser() !== null);

  constructor() {
    this.restoreSession();
  }

  private restoreSession(): void {
    const token = localStorage.getItem(ACCESS_TOKEN_KEY);
    if (!token) return;
    try {
      // Decode JWT payload (base64url) without verification — server validates on every request
      const payload = JSON.parse(atob(token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/')));
      const expiresAt = payload.exp * 1000;
      if (Date.now() >= expiresAt) {
        this.clearSession();
        return;
      }
      this.currentUser.set({
        userId: payload.sub,
        email: payload.email,
        displayName: payload.displayName ?? '',
        roles: Array.isArray(payload.role) ? payload.role : (payload.role ? [payload.role] : [])
      });
    } catch {
      this.clearSession();
    }
  }

  register(req: RegisterRequest): Observable<TokenPair> {
    return this.http.post<TokenPair>('/api/auth/register', req).pipe(
      tap(tokens => this.storeTokens(tokens))
    );
  }

  login(req: LoginRequest): Observable<TokenPair> {
    return this.http.post<TokenPair>('/api/auth/login', req).pipe(
      tap(tokens => this.storeTokens(tokens))
    );
  }

  logout(): Observable<void> {
    const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY) ?? '';
    return this.http.post<void>('/api/auth/logout', { refreshToken }).pipe(
      tap(() => this.clearSession())
    );
  }

  refreshTokens(): Observable<TokenPair> {
    const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY) ?? '';
    return this.http.post<TokenPair>('/api/auth/refresh', { refreshToken }).pipe(
      tap(tokens => this.storeTokens(tokens))
    );
  }

  getAccessToken(): string | null {
    return localStorage.getItem(ACCESS_TOKEN_KEY);
  }

  clearSession(): void {
    localStorage.removeItem(ACCESS_TOKEN_KEY);
    localStorage.removeItem(REFRESH_TOKEN_KEY);
    this.currentUser.set(null);
  }

  private storeTokens(tokens: TokenPair): void {
    localStorage.setItem(ACCESS_TOKEN_KEY, tokens.accessToken);
    localStorage.setItem(REFRESH_TOKEN_KEY, tokens.refreshToken);
    this.restoreSession();
  }
}
