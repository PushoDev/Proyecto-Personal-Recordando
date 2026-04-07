using System;

namespace Domain.Entidades
{
    public class ClickLog
    {
        public int Id { get; set; }
        public int RecursoId { get; set; }
        public DateTime FechaHora { get; set; }
        public string? IpOrigen { get; set; }

        // Navegación (opcional para EF Core)
        public Recurso? Recurso { get; set; }

        private ClickLog() { }

        public ClickLog(int recursoId, string? ipOrigen = null)
        {
            if (recursoId <= 0)
                throw new ArgumentOutOfRangeException(nameof(recursoId), "El ID del recurso debe ser positivo.");

            RecursoId = recursoId;
            FechaHora = DateTime.UtcNow;
            IpOrigen = ipOrigen?.Trim();
        }
    }
}