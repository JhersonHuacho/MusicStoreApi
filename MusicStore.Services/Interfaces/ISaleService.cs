using MusicStore.Dto.Request;
using MusicStore.Dto.Response;

namespace MusicStore.Services.Interfaces;

public interface ISaleService
{
	Task<BaseResponseGeneric<int>> AddAsync(string email, SaleRequestDto saleRequestDto);
	Task<BaseResponseGeneric<SaleResponseDto>> GetAsync(int id);		
	Task<BaseResponseGeneric<ICollection<SaleResponseDto>>> GetAsync(SaleByDateSearchDto saleByDateSearchDto, PaginationDto paginationDto);
	Task<BaseResponseGeneric<ICollection<SaleResponseDto>>> GetAsync(string email, string title, PaginationDto paginationDto);
}
