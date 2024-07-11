using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Services.Interfaces;

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
		return response.Success ? Ok(response) : BadRequest(response);
	}
}
