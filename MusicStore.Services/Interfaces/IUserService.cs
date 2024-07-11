using MusicStore.Dto.Request;
using MusicStore.Dto.Response;

namespace MusicStore.Services.Interfaces;

public interface IUserService
{
	Task<BaseResponseGeneric<RegisterResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto);
	Task<BaseResponseGeneric<LoginResponseDto>> LoginAsync(LoginRequestDto loginRequestDto);
}
