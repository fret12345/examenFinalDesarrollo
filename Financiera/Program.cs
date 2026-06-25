
using Financiera.Api.Middleware;
using Financiera.Application.Interface.Repository;
using Financiera.Application.Interface.Service;
using Financiera.Application.Mappings;
using Financiera.Application.Service;
using Financiera.Domain.Entities;
using Financiera.Infrestructure.Data;
using Financiera.Infrestructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno
DotNetEnv.Env.Load();
builder.Configuration.AddEnvironmentVariables();

// Leer las variables de entorno
var host = Environment.GetEnvironmentVariable("HOST");
var port = Environment.GetEnvironmentVariable("PORT");
var database = Environment.GetEnvironmentVariable("DATABASE");
var user = Environment.GetEnvironmentVariable("USER");
var password = Environment.GetEnvironmentVariable("PASSWORD");
var key = Environment.GetEnvironmentVariable("JWT_KEY");
var issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

// Validar variables de entorno
var variablesFaltantes = new List<string>();
if (string.IsNullOrEmpty(host)) variablesFaltantes.Add("HOST");
if (string.IsNullOrEmpty(port)) variablesFaltantes.Add("PORT");
if (string.IsNullOrEmpty(database)) variablesFaltantes.Add("DATABASE");
if (string.IsNullOrEmpty(user)) variablesFaltantes.Add("USER");
if (string.IsNullOrEmpty(password)) variablesFaltantes.Add("PASSWORD");

if (variablesFaltantes.Any())
{
    throw new Exception($"Faltan variables de entorno: {string.Join(", ", variablesFaltantes)}");
}

// Construir la cadena de conexión
var connectionString =
    $"Host={host};" +
    $"Port={port};" +
    $"Database={database};" +
    $"Username={user};" +
    $"Password={password};" +
    $"SSL Mode=Require;" +
    $"Trust Server Certificate=true;";

// Registrar ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(optios =>
{
    optios.UseNpgsql(connectionString);
});

//Definir reglas de seguridaad
builder.Services.AddIdentity<Usuarios, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Registrar repositorios con sus interfaces
builder.Services.AddScoped<IUsuarioRepositoty, UsuarioRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IRutasRepository, RutasRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IPrestamoRepository, PrestamoRepository>();
builder.Services.AddScoped<ICuotasRepository, CuotaRepository>();
builder.Services.AddScoped<ISeederRepository, SeederRepository>();
builder.Services.AddScoped<ICuetnasPorCobrarRepository, CuentaPorCobrarRepository>();
builder.Services.AddScoped<IPagosRepository, PagosRepository>();

// Registrar servicios con sus interfaces
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AunthService>();
builder.Services.AddScoped<IRutasService, RutasService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPrestamoService, PrestamoService>();
builder.Services.AddScoped<ICuotaService, CuotaService>();
builder.Services.AddScoped<ICuentasPorCobrarService, CuentaPorCobrarService>();
builder.Services.AddScoped<IPagosService, PagosService>();

//Configurar la autenticacion
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}
).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        RoleClaimType = ClaimTypes.Role,
        ValidIssuer = issuer,
        ValidAudience = audience,
    };

    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.HandleResponse();

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = 401,
                datial = "no autenticado. El token es invelido o no fue enviado"
            }));
        },
        
        OnForbidden = async context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = 403,
                datial = "no autorizado. No tienes permisos para acceder a este recurso"
            }));
        }
    };
});



// Registrar AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile).Assembly);

// Add services to the container.

builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Financiera API",
        Description = "API para la gestión financiera",
        Contact = new OpenApiContact
        {
            Name = "Alexander Ruiz",
            Email = "Alex@gmail.com"
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Autenticacion",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT. Ejemplo: eyjhbGcJHByusdUNDBYSjsubs84d..."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference(referenceId: "Bearer", hostDocument: document),
            new List<string>()
        }
    });
});

//Confiiguracion del CORS
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.WithOrigins(
               "http://localhost:4200", //Angular frontend
               "http://localhost:3000" //React
       )
              .AllowAnyMethod()
              .AllowAnyHeader();
        }
        else
        {
            // solo para desarrollo si no hay configuracion de frontend en produccion
            policy.AllowAnyHeader()
            .AllowAnyMethod();
        }

    });
});


builder.Services.AddOpenApi();
 
// construir la aplicacion
var app = builder.Build();

// Registrar middleware para excepciones globales
app.UseMiddleware<ExceptionMiddleware>();


//app.UseHttpsRedirection();

app.UseAuthorization();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        if (await context.Database.CanConnectAsync())
        {
            var seeder = services.GetRequiredService<ISeederRepository>();
            await seeder.SeedAsync();
            logger.LogInformation("El seeder se ejecutó correctamente.");
        }
        else
        {
            logger.LogWarning("No se pudo conectar a la base de datos. Asegúrese de que esté creada y el servicio activo. El seeder no se ejecutará.");
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ocurrió un error al ejecutar el seeder de base de datos.");
    }
}

// Configuracion para entorno de desarrollo y produccion
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Financiera API v1");
});

app.MapGet("/", context =>
{
    context.Response.Redirect("swagger/index.html");
    return Task.CompletedTask;
});

app.UseCors("FrontendPolicy");
 
//mapearControladores
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.Run();
}
else
{
    var apiPort = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    app.Run($"http://0.0.0.0:{apiPort}");
}


