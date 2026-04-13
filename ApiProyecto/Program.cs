
using Infraestructura.Data;
using Infraestructura.Repositories;
using Domain.Interfaces;
using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Yarp.ReverseProxy;

namespace ApiProyecto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });
            builder.Services.AddReverseProxy()
                .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configurar ASP.NET Core Identity
            builder.Services.AddIdentity<Domain.Entidades.ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<RecursoDbContext>()
            .AddDefaultTokenProviders();

            // Configurar JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "ProyectoPersonal",
                    ValidAudience = builder.Configuration["Jwt:Audience"] ?? "ProyectoPersonalUsers",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "SuperSecretKey123456789012345678901234567890"))
                };
            });

            // Configurar EF Core
            builder.Services.AddDbContext<RecursoDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configurar repositorios
            builder.Services.AddScoped<IRecursoRepository, RecursoRepository>();

            // Registrar servicio de autenticación
            builder.Services.AddScoped<IAuthService, AuthService>();

            // Configurar CORS para permitir peticiones desde el frontend durante el desarrollo
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    var frontendUrl = builder.Configuration["Frontend:DevUrl"] ?? "http://localhost:3000";
                    // Allow common dev origins (Vite default port in this project is 11840)
                    policy.WithOrigins(frontendUrl, "http://localhost:11840")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

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

            // YARP Reverse Proxy
            app.MapReverseProxy();

            // Habilitar CORS usando la política configurada
            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
