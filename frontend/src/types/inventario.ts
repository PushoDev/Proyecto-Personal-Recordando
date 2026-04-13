export interface RecursoDTO {
  id: number;
  nombre: string;
  descripcion?: string;
  stock: number;
  umbralMinimo: number;
  urlOriginal?: string;
  codigoCorto?: string;
  clicks: number;
  estaEnEstadoCritico: boolean;
  fechaCreacion: string;
  fechaVencimiento?: string;
  prioridad: number;
  estado: number;
  estaVencida: boolean;
  esCritica: boolean;
}

export interface CreateRecursoRequest {
  nombre: string;
  descripcion?: string;
  stockInicial: number;
  umbralMinimo: number;
  fechaVencimiento?: string;
  prioridad: number;
}

export interface DescontarStockRequest {
  cantidad: number;
}

export const PrioridadLabels: Record<number, string> = {
  0: 'Baja',
  1: 'Media',
  2: 'Alta'
};

export const EstadoLabels: Record<number, string> = {
  0: 'Pendiente',
  1: 'En Progreso',
  2: 'Completada'
};

export const PrioridadColors: Record<number, 'success' | 'warning' | 'error'> = {
  0: 'success',
  1: 'warning',
  2: 'error'
};

export const EstadoColors: Record<number, 'default' | 'warning' | 'success'> = {
  0: 'default',
  1: 'warning',
  2: 'success'
};