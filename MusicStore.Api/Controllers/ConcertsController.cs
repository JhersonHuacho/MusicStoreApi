using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Entities;
using MusicStore.Services.Interfaces;

namespace MusicStore.Api.Controllers;

[ApiController]
[Route("api/concerts")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
public class ConcertsController : ControllerBase
{
	private readonly IConcertService _concertService;
	private readonly ILogger<ConcertsController> _logger;

	public ConcertsController(IConcertService concertService, ILogger<ConcertsController> logger)
    {
		_concertService = concertService;
		_logger = logger;
	}

	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var response = await _concertService.GetAsync(1);
		return response.Success ? Ok(response) : BadRequest(response);
	}

	[HttpGet("title")]
	[AllowAnonymous]
	public async Task<IActionResult> Get(string? title, [FromQuery] PaginationDto paginationDto)
	{
		var response = await _concertService.GetAsync(title, paginationDto);
		return response.Success ? Ok(response) : BadRequest(response);
	}

	[HttpGet("title-lazing-loadin")]
	public async Task<IActionResult> GetWithLazingLoading(string? title, PaginationDto paginationDto)
	{
		var response = await _concertService.GetAsync(title, paginationDto);
		return response.Success ? Ok(response) : BadRequest(response);
	}

	[HttpGet("title-with-stored")]
	public async Task<IActionResult> GetLazin(string? title, PaginationDto paginationDto)
	{
		var response = await _concertService.GetAsync(title, paginationDto);
		return response.Success ? Ok(response) : BadRequest(response);
	}

	[HttpGet("{id:int}")]
	[AllowAnonymous]
	public async Task<IActionResult> Get(int id)
	{
		var response = await _concertService.GetAsync(id);
		return response.Success ? Ok(response) : NotFound(response);
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromForm] ConcertRequestDto concertRequestDto)
	{
		var response = await _concertService.AddAsync(concertRequestDto);
		return response.Success ? Ok(response) : BadRequest(response);
	}

	[HttpPut("{id:int}")]
	public async Task<IActionResult> Put(int id, [FromForm] ConcertRequestDto concertRequestDto)
	{
		var response = await _concertService.UpdateAsync(id, concertRequestDto);
		return response.Success ? Ok(response) : BadRequest(response);
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var response = await _concertService.DeleteAsync(id);
		return response.Success ? Ok(response) : BadRequest(response);
	}

	[HttpPatch("{id:int}/finalize")]
	public async Task<IActionResult> PatchFinalize(int id)
	{
		var response = await _concertService.FinalizeAsync(id);
		return Ok(response);
	}
}
