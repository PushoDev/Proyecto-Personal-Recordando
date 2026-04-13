export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  nombreCompleto: string;
  email: string;
  password: string;
  confirmPassword?: string;
}

export interface AuthResponse {
  success: boolean;
  message: string;
  token: string;
  refreshToken: string;
  expiration: string;
  user: UserDto;
}

export interface UserDto {
  id: string;
  email: string;
  nombreCompleto: string;
  activo: boolean;
  fechaRegistro: string;
}