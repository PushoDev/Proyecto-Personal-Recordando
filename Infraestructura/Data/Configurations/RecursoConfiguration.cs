using Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infraestructura.Data.Configurations
{
    public class RecursoConfiguration : IEntityTypeConfiguration<Recurso>
    {
        public void Configure(EntityTypeBuilder<Recurso> builder)
        {
            builder.ToTable("Recursos");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(r => r.Descripcion)
                .HasMaxLength(1000);

            // Campos de inventario
            builder.Property(r => r.Stock)
                .IsRequired();

            builder.Property(r => r.UmbralMinimo)
                .IsRequired();

            builder.Property(r => r.UrlOriginal)
                .HasMaxLength(2000);

            builder.Property(r => r.CodigoCorto)
                .HasMaxLength(50);

            builder.Property(r => r.Clicks)
                .IsRequired()
                .HasDefaultValue(0);

            // Campos de tarea
            builder.Property(r => r.FechaCreacion)
                .IsRequired();

            builder.Property(r => r.FechaVencimiento);

            builder.Property(r => r.Prioridad)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(r => r.Estado)
                .IsRequired()
                .HasDefaultValue(0);

            // Índice único para CodigoCorto
            builder.HasIndex(r => r.CodigoCorto)
                .IsUnique()
                .HasFilter("[CodigoCorto] IS NOT NULL");

            // Índice para búsquedas por nombre
            builder.HasIndex(r => r.Nombre);

            // Índice para filtros de tareas
            builder.HasIndex(r => r.Estado);
            builder.HasIndex(r => r.Prioridad);
            builder.HasIndex(r => r.FechaVencimiento);
        }
    }
}