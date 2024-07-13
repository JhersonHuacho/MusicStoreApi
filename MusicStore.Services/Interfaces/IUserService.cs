using MusicStore.Dto.Request;
using MusicStore.Dto.Response;

namespace MusicStore.Services.Interfaces;

public interface IUserService
{
	Task<BaseResponseGeneric<RegisterResponseDto>> RegisterAsync(RegisterRequestDto requestDto);
	Task<BaseResponseGeneric<LoginResponseDto>> LoginAsync(LoginRequestDto requestDto);
	Task<BaseResponse> RequestTokenToResetPasswordAsync(ResetPasswordRequestDto requestDto);
	Task<BaseResponse> ResetPasswordAsync(NewPasswordRequestDto requestDto);
	Task<BaseResponse> ChangePasswordAsync(string email, ChangePasswordRequestDto requestDto);
}
