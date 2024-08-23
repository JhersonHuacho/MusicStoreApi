using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using MusicStore.Api.Configurations;
using MusicStore.Api.Filters;
using MusicStore.Entities;

var builder = WebApplication.CreateBuilder(args);
// Options pattern registration
//builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("JWT"));
// Mapeamos el archivo appsettings{environment}.json en una clase de tipo AppSettings
// builder.Configuration es una instancia de IConfiguration que contiene todos los valores de configuración de la aplicación.
// Estos valores pueden provenir de varios orígenes, como archivos JSON (por ejemplo, appsettings.json), variables de entorno, argumentos de línea de comandos, etc.
builder.Services.Configure<AppSettings>(builder.Configuration);
//  Configure CORS
var corsConfiguration = "MusicStoreCors";//builder.Configuration.GetSection("Cors").Get<string[]>();
builder.Services.AddCorsPolicy(corsConfiguration);
// Add services to the container.
builder.Services.AddControllers(options => 
{
	options.Filters.Add(typeof(FilterExceptions));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configuring Context
builder.Services.AddDatabaseConfigurationService(builder.Configuration);
// Configure Jwt Authentication
builder.Services.AddJwtAuthenticationServices(builder.Configuration);
// Configure Identity
builder.Services.AddAuthorization();
builder.Services.AddIdentityServices();
// AddHttpContextAccessor for HttpContext injection nos permite inyectar el HttpContext en cualquier parte de la aplicación
builder.Services.AddHttpContextAccessor();
// Registering services
builder.Services.AddApplicationServices();
// Registering healthchecks
builder.Services.AddHealthChecksServices();
builder.Services.AddAutoMapperServices();

var app = builder.Build();
app.UseMiddlewareExtensions(corsConfiguration);
app.UseMiddlewareEndpoints();
// Scope for Auto-Migrations
await app.Services.ApplyMigrationsAsync();
// Configuring health checks
app.UseHealthChecks("/healthcheck", new HealthCheckOptions()
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.Run();
