export interface LoginRequestDto {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  tokenExpired: number;
  refreshToken: string;
}

export interface Salon {
  id: string;
  name: string;
  logoPath: string;
}

export type SalonsResponse = Salon[];

export interface AddSalonForm {
  name: string;
  logo: File;
}
