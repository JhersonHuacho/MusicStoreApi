using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;

namespace MusicStore.Repositories
{
	public interface IGenreRespository
	{
		Task<int> AddAsync(GenreRequestDto genre);
		Task DeleteAsync(int id);
		Task<IEnumerable<GenreResponseDto>> GetAsync();
		Task<GenreResponseDto?> GetAsync(int id);
		Task UpdateAsync(int id, GenreRequestDto genre);
	}
}