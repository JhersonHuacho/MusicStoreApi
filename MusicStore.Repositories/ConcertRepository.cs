﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MusicStore.Dto.Request;
using MusicStore.Entities;
using MusicStore.Entities.Info;
using MusicStore.Persistence;
using MusicStore.Repositories.Utils;

namespace MusicStore.Repositories;

public class ConcertRepository : RepositoryBase<Concert>, IConcertRepository
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public ConcertRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
	{
		_httpContextAccessor = httpContextAccessor;
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

	public async Task<ICollection<ConcertInfo>> GetAsync(string? title, PaginationDto paginationDto)
	{
		// Eager loading approach optimized to include the related entities
		// trata de cargar entidades relacionadas de manera anticipada (en una sola consulta) de manera optimizada
		// para evitar la carga de datos innecesarios
		var queryable = _context.Set<Concert>()
			.Include(x => x.Genre)
			.Where(x => x.Title.Contains(title ?? string.Empty))
			.IgnoreQueryFilters()
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
				DateEvent = x.DateEvent.ToShortDateString(),
				TimeEvent = x.DateEvent.ToShortTimeString(),
				ImageUrl = x.ImageUrl,
				TicketsQuantity = x.TicketsQuantity,
				Finalized = x.Finalized,
				Status = x.Status ? "Activo" : "Inactivo"
			})				
			.AsQueryable();

		await _httpContextAccessor.HttpContext.InsertPaginationHeader(queryable);

		var response = await queryable.OrderBy(x => x.Id)
			.Paginate(paginationDto)
			.ToListAsync();

		return response;
	}
	public async Task<ICollection<ConcertInfo>> GetLazingAsync(string? title)
	{
		// Lazy loading approach es  	
		return await _context.Set<Concert>()				
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
				DateEvent = x.DateEvent.ToShortDateString(),
				TimeEvent = x.DateEvent.ToShortTimeString(),
				ImageUrl = x.ImageUrl,
				TicketsQuantity = x.TicketsQuantity,
				Finalized = x.Finalized,
				Status = x.Status ? "Activo" : "Inactivo"
			})
			.ToListAsync();
	}

	public async Task<ICollection<ConcertInfo>> GetWithStoredAsync(string? title)
	{			
		var query = _context.Set<ConcertInfo>().FromSqlRaw("dbo.usp_ListConcerts {0}", title ?? string.Empty);
		return await query.ToListAsync();
	}

	public async Task FinalizedAsync(int id)
	{
		var entity = await GetAsync(id);
		if (entity is not null)
		{
			entity.Finalized = true;
			await UpdateAsync();
		}
	}
}
