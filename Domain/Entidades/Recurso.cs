using System;

namespace Domain.Entidades
{
    public class Recurso
    {
        public int Id { get; set; }

        // Campos básicos
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        
        // Campos de inventario
        public int Stock { get; set; }
        public int UmbralMinimo { get; set; }
        public string? UrlOriginal { get; set; }
        public string? CodigoCorto { get; set; }
        public int Clicks { get; set; }
        
        // Campos de tarea (para gestor de tareas)
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int Prioridad { get; set; }
        public int Estado { get; set; }

        private Recurso() { }

        // Constructor para inventario
        public Recurso(string nombre, int stockInicial, int umbralMinimo)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));

            Nombre = nombre.Trim();
            Stock = stockInicial;
            UmbralMinimo = umbralMinimo;
            FechaCreacion = DateTime.Now;
            Estado = 0;
            Prioridad = 1;
        }

        // Constructor para tarea
        public Recurso(string nombre, string? descripcion, DateTime? fechaVencimiento, int prioridad)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));

            Nombre = nombre.Trim();
            Descripcion = descripcion?.Trim();
            FechaCreacion = DateTime.Now;
            FechaVencimiento = fechaVencimiento;
            Prioridad = prioridad;
            Estado = 0;
        }

        // Métodos de inventario
        public void DescontarStock(int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(cantidad));
            if (cantidad > Stock)
                throw new InvalidOperationException("No hay suficiente stock disponible.");
            Stock -= cantidad;
        }

        public void AgregarStock(int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(cantidad));
            Stock += cantidad;
        }

        public void ActualizarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));
            Nombre = nombre.Trim();
        }

        public void AjustarUmbralMinimo(int umbralMinimo)
        {
            if (umbralMinimo < 0)
                throw new ArgumentOutOfRangeException(nameof(umbralMinimo), "El umbral mínimo no puede ser negativo.");
            UmbralMinimo = umbralMinimo;
        }

        public bool EstaEnEstadoCritico() => Stock <= UmbralMinimo;
        public void RegistrarClick() => Clicks++;

        public void ConfigurarUrlCorta(string urlOriginal, string codigoCorto)
        {
            if (string.IsNullOrWhiteSpace(urlOriginal))
                throw new ArgumentException("La URL original es requerida.", nameof(urlOriginal));
            if (string.IsNullOrWhiteSpace(codigoCorto))
                throw new ArgumentException("El código corto es requerido.", nameof(codigoCorto));
            UrlOriginal = urlOriginal.Trim();
            CodigoCorto = codigoCorto.Trim();
        }

        // Métodos de tarea
        public void ActualizarTarea(string nombre, string? descripcion, DateTime? fechaVencimiento, int prioridad)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es requerido.", nameof(nombre));
            Nombre = nombre.Trim();
            Descripcion = descripcion?.Trim();
            FechaVencimiento = fechaVencimiento;
            Prioridad = prioridad;
        }

        public void MarcarCompletada() => Estado = 2;
        public void MarcarEnProgreso() => Estado = 1;
        public void MarcarPendiente() => Estado = 0;

        public bool EstaVencida() => Estado != 2 && FechaVencimiento.HasValue && DateTime.Now > FechaVencimiento.Value;
        public bool EsCritica() => Estado != 2 && Prioridad == 2;
    }
}