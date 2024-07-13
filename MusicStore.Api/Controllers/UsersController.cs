using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Services.Interfaces;
using System.Security.Claims;

namespace MusicStore.Api.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly IUserService _userService;
	public UsersController(IUserService userService)
	{
		_userService = userService;
	}

	[HttpPost("Register")]
	public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
	{
		var response = await _userService.RegisterAsync(registerRequestDto);
		return response.Success ? Ok(response) : BadRequest(response);
	}

	[HttpPost("Login")]
	public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
	{
		var response = await _userService.LoginAsync(loginRequestDto);
		return response.Success ? Ok(response) : Unauthorized(response);
	}

	[HttpPost("RequestTokenToResetPassword")]
	public async Task<IActionResult> RequestTokenToResetPassword([FromBody] ResetPasswordRequestDto resetPasswordRequestDto)
	{
		var response = await _userService.RequestTokenToResetPasswordAsync(resetPasswordRequestDto);
		return response.Success ? Ok(response) : BadRequest(response);
	}

	[HttpPost("ResetPassword")]
	public async Task<IActionResult> ResetPassword([FromBody] NewPasswordRequestDto newPasswordRequestDto)
	{
		var response = await _userService.ResetPasswordAsync(newPasswordRequestDto);
		return response.Success ? Ok(response) : BadRequest(response);
	}

	[HttpPost("ChangePassword")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto changePasswordRequestDto)
	{
		var email = HttpContext.User.Claims.First(p => p.Type == ClaimTypes.Email).Value;
		var response = await _userService.ChangePasswordAsync(email, changePasswordRequestDto);

		return response.Success ? Ok(response) : BadRequest(response);
	}
}
