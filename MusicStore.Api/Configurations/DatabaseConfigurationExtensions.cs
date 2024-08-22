using Microsoft.EntityFrameworkCore;
using MusicStore.Persistence;

namespace MusicStore.Api.Configurations
{
	public static class DatabaseConfigurationExtensions
	{
		public static IServiceCollection AddDatabaseConfigurationService(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<ApplicationDbContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
			});

			return services;
		}
		public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider)
		{
			// Aplicar migraciones automáticamente

			// Scope for Auto-Migrations
			using (var scope = serviceProvider.CreateScope())
			{
				// Auto-Migrations 
				// Automatically apply any pending migrations
				var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
				await applicationDbContext.Database.MigrateAsync();

				// Seed Data
				await UserDataSeeder.Seed(scope.ServiceProvider);
			};
		}
	}
}
