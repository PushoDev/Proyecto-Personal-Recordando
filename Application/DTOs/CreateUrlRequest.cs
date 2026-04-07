namespace Application.DTOs
{
    public class CreateUrlRequest
    {
        public string UrlOriginal { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int StockInicial { get; set; } = 100; // Valor por defecto
        public int UmbralMinimo { get; set; } = 10;  // Valor por defecto
    }
}