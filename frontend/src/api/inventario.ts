import { RecursoDTO, CreateRecursoRequest } from '../types/inventario';

const API_URL = '/api/recursos';

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

export const inventarioApi = {
  getAll: async (): Promise<RecursoDTO[]> => {
    const response = await fetch(API_URL);
    return handleResponse(response);
  },

  getById: async (id: number): Promise<RecursoDTO> => {
    const response = await fetch(`${API_URL}/${id}`);
    return handleResponse(response);
  },

  create: async (data: CreateRecursoRequest): Promise<RecursoDTO> => {
    const response = await fetch(API_URL, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },

  update: async (id: number, data: CreateRecursoRequest): Promise<RecursoDTO> => {
    const response = await fetch(`${API_URL}/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(data),
    });
    return handleResponse(response);
  },

  delete: async (id: number): Promise<void> => {
    const response = await fetch(`${API_URL}/${id}`, {
      method: 'DELETE',
    });
    if (!response.ok) {
      const text = await response.text();
      throw new Error(text || 'Error al eliminar');
    }
  },

  descontarStock: async (id: number, cantidad: number): Promise<RecursoDTO> => {
    const response = await fetch(`${API_URL}/${id}/stock/descontar`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ cantidad }),
    });
    return handleResponse(response);
  },

  agregarStock: async (id: number, cantidad: number): Promise<RecursoDTO> => {
    const response = await fetch(`${API_URL}/${id}/stock/agregar`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ cantidad }),
    });
    return handleResponse(response);
  },
};