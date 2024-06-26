﻿using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories;
using System.Net;

namespace MusicStore.Api.Controllers
{
	[ApiController]
	[Route("api/genres")]
	public class GenresController : ControllerBase
	{
		private readonly IGenreRespository _genreRespository;
		private readonly ILogger<GenresController> _logger;

		public GenresController(IGenreRespository genreRespository, ILogger<GenresController> logger)
        {
			_genreRespository = genreRespository;
			_logger = logger;
		}

		[HttpGet]
		//public async Task<ActionResult<List<Genre>> Get()
		public async Task<IActionResult> Get()
		{
			var response = new BaseResponseGeneric<IEnumerable<GenreResponseDto>>();
			try
			{
				response.Data = await _genreRespository.GetAsync();
				response.Success = true;
				_logger.LogInformation("Obteniendo todos los géneros musicales.");

				return Ok(response);
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Ocurrío un error al obtener la información.";
				_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
				return BadRequest(response);				
			}
		}

		[HttpGet("{id:int}")]
		public async Task<IActionResult> Get(int id)
		{
			var response = new BaseResponseGeneric<GenreResponseDto>();
			try
			{
				response.Data = await _genreRespository.GetAsync(id);
				response.Success = true;
				_logger.LogInformation($"Obteniendo el género musical con id {id}.");

				return response.Data is null ? NotFound(response) : Ok(response);
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Ocurrío un error al obtener la información.";
				_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
				return BadRequest(response);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] GenreRequestDto genreRequestDto)
		{
			var response = new BaseResponseGeneric<int>();
			try
			{
				var genreId = await _genreRespository.AddAsync(genreRequestDto);
				response.Data = genreId;
				response.Success = true;
				_logger.LogInformation($"Se ha creado el género musical con id {genreId}.");
				return StatusCode((int)HttpStatusCode.Created, response);
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Ocurrío un error al crear la información.";
				_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
				return BadRequest(response);
			}
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> Put(int id, [FromBody] GenreRequestDto genreRequestDto)
		{
			var response = new BaseResponse();
			try
			{
				await _genreRespository.UpdateAsync(id, genreRequestDto);
				response.Success = true;
				_logger.LogInformation($"Se ha actualizado el género musical con id {id}.");
				return Ok(response);
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Ocurrío un error al actualizar la información.";
				_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
				return BadRequest(response);
			}
		}

		[HttpDelete("{id:int}")]
		public  async Task<IActionResult> Delete(int id)
		{
			var response = new BaseResponse();
			try
			{
				await _genreRespository.DeleteAsync(id);
				response.Success = true;
				_logger.LogInformation($"Se ha eliminado el género musical con id {id}.");
				return Ok(response);
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Ocurrío un error al eliminar la información.";
				_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
				return BadRequest(response);
			}
		}
	}
}
