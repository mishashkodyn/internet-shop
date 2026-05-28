import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from './auth.service';
import { TokenPair } from '../models/auth.models';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    localStorage.clear();
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should start unauthenticated when no token in localStorage', () => {
    expect(service.isAuthenticated()).toBeFalse();
    expect(service.currentUser()).toBeNull();
  });

  it('login() should store tokens and update currentUser', () => {
    // Build a minimal JWT with a known payload (exp far in the future)
    const payload = { sub: 'user-1', email: 'test@test.com', displayName: 'Tester', role: ['User'], exp: Math.floor(Date.now() / 1000) + 3600 };
    const encodedPayload = btoa(JSON.stringify(payload)).replace(/\+/g, '-').replace(/\//g, '_').replace(/=+$/, '');
    const fakeJwt = `header.${encodedPayload}.signature`;

    const mockTokens: TokenPair = {
      accessToken: fakeJwt,
      refreshToken: 'refresh-token',
      expiresAt: new Date(Date.now() + 3600000).toISOString()
    };

    service.login({ email: 'test@test.com', password: 'Password1!' }).subscribe(tokens => {
      expect(tokens.accessToken).toBe(fakeJwt);
    });

    const req = httpMock.expectOne('/api/auth/login');
    expect(req.request.method).toBe('POST');
    req.flush(mockTokens);

    expect(service.isAuthenticated()).toBeTrue();
    expect(service.currentUser()?.email).toBe('test@test.com');
  });

  it('clearSession() should remove tokens and set currentUser to null', () => {
    localStorage.setItem('access_token', 'some-token');
    localStorage.setItem('refresh_token', 'some-refresh');
    service.clearSession();
    expect(service.currentUser()).toBeNull();
    expect(localStorage.getItem('access_token')).toBeNull();
    expect(localStorage.getItem('refresh_token')).toBeNull();
  });
});
