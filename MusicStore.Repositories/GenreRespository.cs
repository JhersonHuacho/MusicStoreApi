using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Persistence;

namespace MusicStore.Repositories
{
	public class GenreRespository : IGenreRespository
	{
		private readonly ApplicationDbContext _applicationDbContext;

		public GenreRespository(ApplicationDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		public async Task<IEnumerable<GenreResponseDto>> GetAsync()
		{
			var items = await _applicationDbContext.Genres.ToListAsync();
			// Mapping from Genre to GenreResponseDto
			var genreResponseDto = items.Select(x => new GenreResponseDto
			{
				Id = x.Id,
				Name = x.Name,
				Status = x.Status
			});

			return genreResponseDto;
		}

		public async Task<GenreResponseDto?> GetAsync(int id)
		{
			var item = await _applicationDbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);
			var genreResponseDto = item is not null ? new GenreResponseDto
			{
				Id = item.Id,
				Name = item.Name,
				Status = item.Status
			} : throw new InvalidOperationException($"No se encontró el registro con id {id}");

			return genreResponseDto;
		}

		public async Task<int> AddAsync(GenreRequestDto genreRequestDto)
		{
			var genre = new Genre
			{
				Name = genreRequestDto.Name,
				Status = genreRequestDto.Status
			};

			_applicationDbContext.Genres.Add(genre);
			await _applicationDbContext.SaveChangesAsync();
			return genre.Id;
		}

		public async Task UpdateAsync(int id, GenreRequestDto genreRequestDto)
		{
			var item = await _applicationDbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);

			if (item is not null)
			{
				item.Name = genreRequestDto.Name;
				item.Status = genreRequestDto.Status;
				_applicationDbContext.Genres.Update(item);
				await _applicationDbContext.SaveChangesAsync();
			}
			else
			{
				throw new InvalidOperationException($"No se encontró el registro con id {id}");
			}
		}

		public async Task DeleteAsync(int id)
		{
			var item = await _applicationDbContext.Genres.FirstOrDefaultAsync(x => x.Id == id);

			if (item is not null)
			{
				_applicationDbContext.Genres.Remove(item);
				await _applicationDbContext.SaveChangesAsync();
			}
			else
			{
				throw new InvalidOperationException($"No se encontró el registro con id {id}");
			}
		}		
	}
}
