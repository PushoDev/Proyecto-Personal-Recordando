namespace Application.DTOs
{
    public class CreateRecursoRequest
    {
        // Campos comunes
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        
        // Campos de inventario (opcionales)
        public int StockInicial { get; set; }
        public int UmbralMinimo { get; set; }
        
        // Campos de tarea (opcionales)
        public DateTime? FechaVencimiento { get; set; }
        public int Prioridad { get; set; } = 1;
    }
}