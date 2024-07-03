using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MusicStore.Persistence;
using MusicStore.Repositories;
using MusicStore.Services.Implementation;
using MusicStore.Services.Interfaces;
using MusicStore.Services.Profiles;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//  Configure CORS
var corsConfiguration = "MusicStoreCors";//builder.Configuration.GetSection("Cors").Get<string[]>();
builder.Services.AddCors(setupAction =>
{
	setupAction.AddPolicy(corsConfiguration, policy =>
	{
		policy.AllowAnyOrigin();
		policy.AllowAnyHeader().WithExposedHeaders(new string[] { "TotalRecordsQuantity" });
		policy.AllowAnyMethod();
	});
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuring Context
builder.Services.AddDbContext<ApplicationDbContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configure Identity
builder.Services.AddAuthentication(x => 
{
	x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => 
{
	var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:JWTKey"]
		?? throw new InvalidOperationException("JWT Key not configured"));

	x.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = false,
		ValidateAudience = false,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		IssuerSigningKey = new SymmetricSecurityKey(key)
	};
});
builder.Services.AddAuthorization();
builder.Services.AddIdentity<MusicStoreUserIdentity, IdentityRole>(policies => 
	{
		policies.Password.RequireDigit = false;
		policies.Password.RequireLowercase = false;
		policies.Password.RequireUppercase = false;		
		policies.Password.RequiredLength = 6;
		policies.User.RequireUniqueEmail = true;
	})
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();

// AddHttpContextAccessor for HttpContext injection nos permite inyectar el HttpContext en cualquier parte de la aplicación
builder.Services.AddHttpContextAccessor();

// Registering services
builder.Services.AddTransient<IGenreRespository,GenreRespository>();
builder.Services.AddTransient<IConcertRepository,ConcertRepository>();
builder.Services.AddTransient<ISaleRepository, SaleRepository>();
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();

builder.Services.AddTransient<IConcertService, ConcertService>();
builder.Services.AddTransient<IGenreService, GenreService>();
builder.Services.AddTransient<ISaleService, SaleService>();

builder.Services.AddAutoMapper(config => 
{
    config.AddProfile<ConcertProfile>();
    config.AddProfile<GenreProfile>();
	config.AddProfile<SaleProfile>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

// Scope for Auto-Migrations
using (var scope = app.Services.CreateScope())
{
	// Auto-Migrations 
	// Automatically apply any pending migrations
	var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
	await applicationDbContext.Database.MigrateAsync();

	// Seed Data
	await UserDataSeeder.Seed(scope.ServiceProvider);
};

app.Run();
