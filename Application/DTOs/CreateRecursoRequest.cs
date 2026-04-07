namespace Application.DTOs
{
    public class CreateRecursoRequest
    {
        public string Nombre { get; set; } = string.Empty;
        public int StockInicial { get; set; }
        public int UmbralMinimo { get; set; }
    }
}