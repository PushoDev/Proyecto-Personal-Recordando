using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class RecursoDTO
    {
        // Campos comunes
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        
        // Campos de inventario
        public int Stock { get; set; }
        public int UmbralMinimo { get; set; }
        public string? UrlOriginal { get; set; }
        public string? CodigoCorto { get; set; }
        public int Clicks { get; set; }
        public bool EstaEnEstadoCritico { get; set; }
        
        // Campos de tarea
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public int Prioridad { get; set; }
        public int Estado { get; set; }
        
        // Propiedades helper - JsonInclude para que se serialicen
        [JsonInclude]
        public bool EstaVencida => Estado != 2 && FechaVencimiento.HasValue && DateTime.Now > FechaVencimiento.Value;
        
        [JsonInclude]
        public bool EsCritica => Estado != 2 && Prioridad == 2;
    }
}