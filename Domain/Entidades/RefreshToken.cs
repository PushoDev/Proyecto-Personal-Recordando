using Domain.Entidades;

namespace Domain.Entidades
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsRevoked { get; set; } = false;

        // Relación de navegación
        public virtual ApplicationUser User { get; set; } = null!;
    }
}