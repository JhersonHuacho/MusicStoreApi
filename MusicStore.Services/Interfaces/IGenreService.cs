using MusicStore.Dto.Request;
using MusicStore.Dto.Response;

namespace MusicStore.Services.Interfaces
{
	public interface IGenreService
	{
		Task<BaseResponseGeneric<ICollection<GenreResponseDto>>> GetAsync();
		Task<BaseResponseGeneric<GenreResponseDto>> GetAsync(int id);
		Task<BaseResponseGeneric<int>> AddAsync(GenreRequestDto genreResponseDto);
		Task<BaseResponse> UpdateAsync(int id, GenreRequestDto genreResponseDto);
		Task<BaseResponse> DeleteAsync(int id);
	}
}
