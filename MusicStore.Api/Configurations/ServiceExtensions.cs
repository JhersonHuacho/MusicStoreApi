using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using MusicStore.Persistence;
using MusicStore.Repositories;
using MusicStore.Services.Implementation;
using MusicStore.Services.Interfaces;
using MusicStore.Services.Profiles;
using System.Text;

namespace MusicStore.Api.Configurations
{
	public static class ServiceExtensions
	{
		public static IServiceCollection AddCorsPolicy(this IServiceCollection services, string corsConfiguration)
		{
			services.AddCors(setupAction =>
			{
				setupAction.AddPolicy(corsConfiguration, policy =>
				{
					policy.AllowAnyOrigin();
					policy.AllowAnyHeader().WithExposedHeaders(new string[] { "TotalRecordsQuantity" });
					policy.AllowAnyMethod();
				});
			});

			return services;
		}

		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddTransient<IGenreRespository, GenreRespository>();
			services.AddTransient<IConcertRepository, ConcertRepository>();
			services.AddTransient<ISaleRepository, SaleRepository>();
			services.AddTransient<ICustomerRepository, CustomerRepository>();
			
			services.AddTransient<IConcertService, ConcertService>();
			services.AddTransient<IGenreService, GenreService>();
			services.AddTransient<ISaleService, SaleService>();
			services.AddTransient<IUserService, UserService>();
			services.AddTransient<IEmailService, EmailService>();

			//builder.Services.AddTransient<IFileStorage, FileStorageAzure>();
			services.AddTransient<IFileStorage, FileStorageLocal>();
			return services;
		}

		public static IServiceCollection AddIdentityServices(this IServiceCollection services)
		{
			services.AddIdentity<MusicStoreUserIdentity, IdentityRole>(policies =>
				{
					policies.Password.RequireDigit = false;
					policies.Password.RequireLowercase = false;
					policies.Password.RequireUppercase = false;
					policies.Password.RequiredLength = 6;
					policies.User.RequireUniqueEmail = true;
				})
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			return services;
		}

		public static IServiceCollection AddJwtAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(x =>
			{
				var key = Encoding.UTF8.GetBytes(configuration["JWT:JWTKey"]
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

			return services;
		}

		public static IServiceCollection AddAutoMapperServices(this IServiceCollection services)
		{
			services.AddAutoMapper(config =>
			{
				config.AddProfile<ConcertProfile>();
				config.AddProfile<GenreProfile>();
				config.AddProfile<SaleProfile>();
			});

			return services;
		}

		public static IServiceCollection AddHealthChecksServices(this IServiceCollection services)
		{
			// Registering healthchecks
			services.AddHealthChecks()
				.AddCheck("selfcheck", () => HealthCheckResult.Healthy())
				.AddDbContextCheck<ApplicationDbContext>();

			return services;
		}
	}
}
