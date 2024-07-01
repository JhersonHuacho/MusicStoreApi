using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Services.Interfaces;

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
	public async Task<IActionResult> Post([FromBody] SaleRequestDto saleRequestDto)
	{		
		var response = await _saleService.AddAsync(saleRequestDto.Email, saleRequestDto);
		if (response.Success)
		{
			return Ok(response);
		}
		return BadRequest(response);
	}
}
