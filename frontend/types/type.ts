export interface LoginRequestDto {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  tokenExpired: number;
  refreshToken: string;
}
