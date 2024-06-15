using Microsoft.EntityFrameworkCore;
using MusicStore.Entities;

namespace MusicStore.Persistence
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}
		// Fluente API es una forma de configurar el modelo de datos en EF Core sin necesidad de usar Data Annotations.
		// y OnModelCreating es un método que se llama cuando el modelo de datos se está creando.
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Genre>().Property(x => x.Name).HasMaxLength(50);
		}
		public DbSet<Genre> Genres { get; set; } = default!;
	}
}
