namespace Application.DTOs
{
    public class RecursoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Stock { get; set; }
        public int UmbralMinimo { get; set; }
        public string? UrlOriginal { get; set; }
        public string? CodigoCorto { get; set; }
        public int Clicks { get; set; }
        public bool EstaEnEstadoCritico { get; set; }
    }
}