using Application.Interfaces;
using Application.Mapping;
using Application.Services;
using Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Middleware;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// ----------------------------
// Configuración de la Base de Datos
// ----------------------------

// Crear carpeta Data dentro del proyecto para almacenar el archivo SQLite
var dataDirectory = Path.Combine(AppContext.BaseDirectory, "Data");

if (!Directory.Exists(dataDirectory))
    Directory.CreateDirectory(dataDirectory);

// Ruta completa del archivo de base de datos
var dbPath = Path.Combine(dataDirectory, "users.db");

// Configuración de conexión SQLite
// SQLite se utiliza por su ligereza en entornos de desarrollo.
// Migrar a SQL Server o PostgreSQL requiere solo cambiar el proveedor y la cadena de conexión.
var connectionString = $"Data Source={dbPath}";
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(connectionString);
});

// ----------------------------
// Inyección de dependencias
// ----------------------------
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

// Registra todos los validadores del ensamblado Application
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

// =========================================
// CONFIGURAR CORS (para React en localhost)
// =========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendDev", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173") // frontend
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Controllers
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserValidator>());


// Swagger para documentación de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "User API",
        Version = "v1",
        Description = "Clean Architecture API with Simple Token Authentication (Simulating Azure AD)"
    });

    // 🔒 Add Bearer token support to Swagger UI
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor copie y pegue el siguiente texto: Bearer my-demo-token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseCors("AllowFrontendDev");

// ----------------------------
// Configuración del middleware
// ----------------------------

// Crear la base de datos si no existe (solo para demo, usar migraciones en producción)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// Middleware de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<TokenAuthenticationMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
