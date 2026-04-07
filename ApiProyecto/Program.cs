
using Infraestructura.Data;
using Infraestructura.Repositories;
using Domain.Interfaces;
using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;

namespace ApiProyecto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configurar EF Core
            builder.Services.AddDbContext<RecursoDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configurar repositorios
            builder.Services.AddScoped<IRecursoRepository, RecursoRepository>();

            // Configurar servicios de aplicación
            builder.Services.AddScoped<IRecursoApplicationService, RecursoApplicationService>();
            builder.Services.AddScoped<IUrlShortenerService>(sp =>
                new UrlShortenerApplicationService(
                    sp.GetRequiredService<IRecursoRepository>(),
                    sp.GetRequiredService<RecursoDbContext>(),
                    sp.GetRequiredService<IConfiguration>(),
                    sp.GetRequiredService<ILogger<UrlShortenerApplicationService>>()
                ));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
