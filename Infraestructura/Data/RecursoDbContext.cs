using Domain.Entidades;
using Infraestructura.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Data
{
    public class RecursoDbContext : IdentityDbContext<ApplicationUser>
    {
        public RecursoDbContext(DbContextOptions<RecursoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Recurso> Recursos { get; set; } = null!;
        public DbSet<ClickLog> ClickLogs { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar configuraciones
            modelBuilder.ApplyConfiguration(new RecursoConfiguration());
            modelBuilder.ApplyConfiguration(new ClickLogConfiguration());
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        }
    }
}