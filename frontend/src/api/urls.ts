export interface UrlDTO {
  id: number;
  nombre: string;
  urlOriginal: string;
  codigoCorto: string;
  clicks: number;
}

export interface CreateUrlRequest {
  nombre: string;
  urlOriginal: string;
}

export interface UrlStatsDTO {
  codigoCorto: string;
  urlOriginal: string;
  clicks: number;
  ultimoClick: string | null;
  clicksHoy: number;
  clicksSemana: number;
  clicksMes: number;
}

const API_URL = '/api/urlshortener';

const getAuthHeaders = () => {
  const token = localStorage.getItem('token');
  return token ? { 'Authorization': `Bearer ${token}` } : {};
};

const handleResponse = async (response: Response) => {
  const text = await response.text();
  if (!response.ok) {
    throw new Error(text || 'Error en la solicitud');
  }
  return text ? JSON.parse(text) : null;
};

export const urlsApi = {
  shorten: async (data: CreateUrlRequest): Promise<UrlDTO> => {
    const response = await fetch(`${API_URL}/shorten`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', ...getAuthHeaders() },
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },

  getStats: async (codigoCorto: string): Promise<UrlStatsDTO> => {
    const response = await fetch(`${API_URL}/${codigoCorto}/stats`, {
      headers: { ...getAuthHeaders() },
    });
    return handleResponse(response);
  },
};