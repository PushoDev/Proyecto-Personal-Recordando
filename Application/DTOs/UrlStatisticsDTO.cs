namespace Application.DTOs
{
    public class UrlStatisticsDTO
    {
        public string CodigoCorto { get; set; } = string.Empty;
        public int TotalClicks { get; set; }
        public int ClicksUltimaHora { get; set; }
        public int RecursoId { get; set; }
        public string NombreRecurso { get; set; } = string.Empty;
        public List<ClickPorDiaDTO> ClicksPorDia { get; set; } = new();
    }

    public class ClickPorDiaDTO
    {
        public DateTime Fecha { get; set; }
        public int CantidadClicks { get; set; }
    }
}