using System;

namespace Domain.Entidades
{
    public class Recurso
    {
        public int Id { get; set; }

        public string Nombre { get; private set; } = string.Empty;

        public int Stock { get; private set; }
        public int UmbralMinimo { get; private set; }

        public string? UrlOriginal { get; private set; }
        public string? CodigoCorto { get; private set; }
        public int Clicks { get; private set; }

        private Recurso() { }

        public Recurso(string nombre, int stockInicial, int umbralMinimo)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre del recurso es requerido.", nameof(nombre));

            if (stockInicial < 0)
                throw new ArgumentOutOfRangeException(nameof(stockInicial), "El stock inicial no puede ser negativo.");

            if (umbralMinimo < 0)
                throw new ArgumentOutOfRangeException(nameof(umbralMinimo), "El umbral mínimo no puede ser negativo.");

            if (umbralMinimo > stockInicial)
                throw new ArgumentException("El umbral mínimo no puede ser mayor que el stock inicial.", nameof(umbralMinimo));

            Nombre = nombre.Trim();
            Stock = stockInicial;
            UmbralMinimo = umbralMinimo;
        }

        public void DescontarStock(int cantidad)
        {
            if (cantidad <= 0)
                throw new ArgumentException("La cantidad debe ser mayor a cero.", nameof(cantidad));

            if (cantidad > Stock)
                throw new InvalidOperationException("No hay suficiente stock disponible.");

            Stock -= cantidad;
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
    }
}
