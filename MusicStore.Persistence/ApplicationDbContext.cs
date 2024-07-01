using Microsoft.EntityFrameworkCore;
using MusicStore.Entities.Info;
using System.Reflection;

namespace MusicStore.Persistence
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}
		// Fluente API: es una forma de configurar el modelo de datos en EF Core sin necesidad de usar Data Annotations.
		// y OnModelCreating es un método que se llama cuando el modelo de datos se está creando.
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			// esto hace que EF Core busque todas las clases que implementan IEntityTypeConfiguration y las aplique															
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			// HasNoKey es un método que se usa para indicar que la entidad no tiene una clave primaria			
			modelBuilder.Entity<ConcertInfo>().HasNoKey();
			
			//modelBuilder.Entity<Genre>().Property(x => x.Name).HasMaxLength(50);
		}
		//public DbSet<Genre> Genres { get; set; } = default!;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				// UseLazyLoadingProxies esto lo que hace es que cuando se cargan entidades relacionadas, estas se cargan de manera diferida
				// es decir, se cargan solo cuando se accede a ellas
				// esto es útil para evitar la carga de datos innecesarios
				optionsBuilder.UseLazyLoadingProxies();
			}
		}
	}
}
