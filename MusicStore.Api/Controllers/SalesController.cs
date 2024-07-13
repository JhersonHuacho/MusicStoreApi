using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Services.Interfaces;
using System.Security.Claims;

namespace MusicStore.Api.Controllers;

[ApiController]
[Route("api/sales")]
public class SalesController : ControllerBase
{
	private readonly ISaleService _saleService;

	public SalesController(ISaleService saleService)
	{
		_saleService = saleService;
	}

	[HttpGet("ListSalesByDate")]
	public async Task<IActionResult> GetByDate([FromQuery] SaleByDateSearchDto saleByDateSearchDto,
		[FromQuery] PaginationDto paginationDto)
	{
		var response = await _saleService.GetAsync(saleByDateSearchDto, paginationDto);
		if (response.Success)
		{
			return Ok(response);
		}
		return BadRequest(response);
	}

	[HttpGet("ListSales")]
	public async Task<IActionResult> Get(string email, [FromQuery] string? title,
		[FromQuery] PaginationDto paginationDto)
	{
		var response = await _saleService.GetAsync(email, title, paginationDto);
		if (response.Success)
		{
			return Ok(response);
		}
		return BadRequest(response);
	}

	[HttpGet("{id:int}")]
	public async Task<IActionResult> Get(int id)
	{
		var response = await _saleService.GetAsync(id);
		if (response.Success)
		{
			return Ok(response);
		}
		return BadRequest(response);
	}

	[HttpPost]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public async Task<IActionResult> Post([FromBody] SaleRequestDto saleRequestDto)
	{
		var email = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
		var response = await _saleService.AddAsync(email, saleRequestDto);
		if (response.Success)
		{
			return Ok(response);
		}
		return BadRequest(response);
	}
}
