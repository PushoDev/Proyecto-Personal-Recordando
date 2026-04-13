import { LoginRequest, RegisterRequest, AuthResponse } from '../types/auth';

const API_URL = '/api/auth';

const handleResponse = async (response: Response) => {
  const text = await response.text();
  let errorMessage = '';
  
  try {
    const json = JSON.parse(text);
    errorMessage = json.message || JSON.stringify(json);
  } catch {
    errorMessage = text;
  }

  if (!response.ok) {
    throw new Error(errorMessage);
  }

  return text ? JSON.parse(text) : null;
};

export const authApi = {
  login: async (data: LoginRequest): Promise<AuthResponse> => {
    const response = await fetch(`${API_URL}/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },

  register: async (data: RegisterRequest): Promise<AuthResponse> => {
    const response = await fetch(`${API_URL}/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },
};