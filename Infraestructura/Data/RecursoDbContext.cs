using Domain.Entidades;
using Infraestructura.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Data
{
    public class RecursoDbContext : DbContext
    {
        public RecursoDbContext(DbContextOptions<RecursoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Recurso> Recursos { get; set; }
        public DbSet<ClickLog> ClickLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar configuraciones
            modelBuilder.ApplyConfiguration(new RecursoConfiguration());
            modelBuilder.ApplyConfiguration(new ClickLogConfiguration());
        }
    }
}