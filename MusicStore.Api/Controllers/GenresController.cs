using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Entities;
using MusicStore.Services.Interfaces;

namespace MusicStore.Api.Controllers
{
	[ApiController]
	[Route("api/genres")]	
	public class GenresController : ControllerBase
	{
		private readonly IGenreService _genreService;
		private readonly ILogger<GenresController> _logger;

		public GenresController(IGenreService genreService, ILogger<GenresController> logger)
        {
			_genreService = genreService;
			_logger = logger;
		}

		[HttpGet]
		//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		//public async Task<IActionResult> Get(PaginationDto paginationDto)
		//public async Task<ActionResult<List<Genre>>> GetAll()
		public async Task<IActionResult> GetAll()
		{
			var response = await _genreService.GetAsync();
			return Ok(response);
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> Get(int id)
		{
			var response = await _genreService.GetAsync(id);
			return response.Success ? Ok(response) : NotFound(response);
		}

		[HttpPost]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
		public async Task<IActionResult> Post([FromBody] GenreRequestDto genreRequestDto)
		{
			var response = await _genreService.AddAsync(genreRequestDto);
			return response.Success ? Ok(response) : BadRequest(response);
		}

		[HttpPut("{id:int}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
		public async Task<IActionResult> Put(int id, [FromBody] GenreRequestDto genreRequestDto)
		{
			var response = await _genreService.UpdateAsync(id, genreRequestDto);
			return response.Success ? Ok(response) : BadRequest(response);
		}

		[HttpDelete("{id:int}")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Constants.RoleAdmin)]
		public  async Task<IActionResult> Delete(int id)
		{
			var response = await _genreService.DeleteAsync(id);
			return response.Success ? Ok(response) : BadRequest(response);
		}
	}
}
