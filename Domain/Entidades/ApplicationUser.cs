using Microsoft.AspNetCore.Identity;

namespace Domain.Entidades
{
    public class ApplicationUser : IdentityUser
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public bool Activo { get; set; } = true;

        // Relaciones de navegación
        public virtual ICollection<Recurso> Recursos { get; set; } = new List<Recurso>();
    }
}