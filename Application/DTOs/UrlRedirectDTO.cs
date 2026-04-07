namespace Application.DTOs
{
    public class UrlRedirectDTO
    {
        public string UrlOriginal { get; set; } = string.Empty;
        public string CodigoCorto { get; set; } = string.Empty;
        public int ClicksTotales { get; set; }
    }
}