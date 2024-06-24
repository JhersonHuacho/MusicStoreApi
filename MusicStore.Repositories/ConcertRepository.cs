using Microsoft.EntityFrameworkCore;
using MusicStore.Entities;
using MusicStore.Entities.Info;
using MusicStore.Persistence;

namespace MusicStore.Repositories
{
	public class ConcertRepository : RepositoryBase<Concert>, IConcertRepository
	{
		public ConcertRepository(ApplicationDbContext context) : base(context)
		{
		}

		public override async Task<ICollection<Concert>> GetAsync()
		{
			// Eager loading approach to include the related entities
			// Eager loading approach se usa para cargar entidades relacionadas de manera anticipada (en una sola consulta)
			return await _context.Set<Concert>()
				.Include(x => x.Genre)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<ICollection<ConcertInfo>> GetAsync(string? title)
		{
			// Eager loading approach optimized to include the related entities
			// trata de cargar entidades relacionadas de manera anticipada (en una sola consulta) de manera optimizada
			// para evitar la carga de datos innecesarios
			return await _context.Set<Concert>()
				.Include(x => x.Genre)
				.Where(x => x.Title.Contains(title ?? string.Empty))
				.AsNoTracking()
				.Select(x => new ConcertInfo
				{
					Id = x.Id,
					Title = x.Title,
					Description = x.Description,
					Place = x.Place,
					UnitPrice = x.UnitPrice,
					GenreId = x.GenreId,
					Genre = x.Genre.Name,
					DataEvent = x.DataEvent.ToShortDateString(),
					TimeEvent = x.DataEvent.ToShortTimeString(),
					ImageUrl = x.ImageUrl,
					TicketsQuantity = x.TicketsQuantity,
					Finalized = x.Finalized,
					Status = x.Status ? "Activo" : "Inactivo"
				})				
				.ToListAsync();
		}
	}
}
