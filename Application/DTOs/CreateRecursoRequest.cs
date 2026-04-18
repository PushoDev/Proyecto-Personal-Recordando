using System.Text.Json.Serialization;

namespace Application.DTOs
{
    public class CreateRecursoRequest
    {
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;
        
        [JsonPropertyName("descripcion")]
        public string? Descripcion { get; set; }
        
        [JsonPropertyName("stockInicial")]
        public int StockInicial { get; set; }
        
        [JsonPropertyName("umbralMinimo")]
        public int UmbralMinimo { get; set; }
        
        [JsonPropertyName("fechaVencimiento")]
        public DateTime? FechaVencimiento { get; set; }
        
        [JsonPropertyName("prioridad")]
        public int Prioridad { get; set; } = 1;
        
        [JsonPropertyName("estado")]
        public int Estado { get; set; }
    }
}