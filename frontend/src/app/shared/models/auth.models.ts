export interface RegisterRequest {
  email: string;
  password: string;
  displayName: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface TokenPair {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
}

export interface UserInfo {
  userId: string;
  email: string;
  displayName: string;
  roles: string[];
}
