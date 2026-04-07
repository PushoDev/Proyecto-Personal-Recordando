using Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configurations
{
    public class ClickLogConfiguration : IEntityTypeConfiguration<ClickLog>
    {
        public void Configure(EntityTypeBuilder<ClickLog> builder)
        {
            builder.ToTable("ClickLogs");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.RecursoId)
                .IsRequired();

            builder.Property(c => c.FechaHora)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(c => c.IpOrigen)
                .HasMaxLength(45); // IPv6 máximo

            // Foreign Key
            builder.HasOne(c => c.Recurso)
                .WithMany()
                .HasForeignKey(c => c.RecursoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices para queries de estadísticas
            builder.HasIndex(c => c.RecursoId);
            builder.HasIndex(c => c.FechaHora);
            builder.HasIndex(c => new { c.RecursoId, c.FechaHora });
        }
    }
}