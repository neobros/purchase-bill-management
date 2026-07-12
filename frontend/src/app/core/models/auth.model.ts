export interface LoginRequest {
  email: string;
  password: string;
}

export interface UserLocation {
  id: number;
  locationCode: string;
  locationName: string;
}

/** Shape of ApiResponse<LoginResponseDto>.data returned by POST /api/auth/login. */
export interface LoginResponseData {
  token: string;
  expiresAt: string;
  email: string;
  locations: UserLocation[];
}

export interface AuthSession {
  token: string;
  expiresAt: string;
  email: string;
  locations: UserLocation[];
}
