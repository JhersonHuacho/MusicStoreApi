using Microsoft.AspNetCore.Mvc;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories;

namespace MusicStore.Api.Controllers
{
	[ApiController]
	[Route("api/concerts")]
	public class ConcertsController : ControllerBase
	{
		private readonly IConcertRepository _concertRepository;
		private readonly IGenreRespository _genreRespository;
		private readonly ILogger<ConcertsController> _logger;

		public ConcertsController(IConcertRepository concertRepository, IGenreRespository genreRespository, 
			ILogger<ConcertsController> logger)
        {
			_concertRepository = concertRepository;
			_genreRespository = genreRespository;
			_logger = logger;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var response = new BaseResponseGeneric<IEnumerable<ConcertResponseDto>>();
			try
			{
				// Mapping
				var concertDb = await _concertRepository.GetAsync();
				var concertsDto = concertDb.Select(x => new ConcertResponseDto
				{
					Title = x.Title,
					Description = x.Description,
					Place = x.Place,
					UnitPrice = x.UnitPrice,
					GenreId = x.GenreId,
					DataEvent = x.DataEvent,
					ImageUrl = x.ImageUrl,
					TicketsQuantity = x.TicketsQuantity,
					Finalized = x.Finalized
				});

				response.Data = concertsDto;
				response.Success = true;
				
				_logger.LogInformation("Obteniendo todos los conciertos.");

				return Ok(response);
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Ocurrío un error al obtener la información.";
				_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
				return BadRequest(response);				
			}
		}

		[HttpGet("title")]
		public async Task<IActionResult> Get(string? title)
		{
			var response = new BaseResponseGeneric<IEnumerable<ConcertResponseDto>>();
			try
			{
				// Mapping
				var concertDb = await _concertRepository
					.GetAsync(x => x.Title.Contains(title ?? string.Empty), x => x.DataEvent);
				var concertsDto = concertDb.Select(x => new ConcertResponseDto
				{
					Title = x.Title,
					Description = x.Description,
					Place = x.Place,
					UnitPrice = x.UnitPrice,
					GenreId = x.GenreId,
					DataEvent = x.DataEvent,
					ImageUrl = x.ImageUrl,
					TicketsQuantity = x.TicketsQuantity,
					Finalized = x.Finalized
				});

				response.Data = concertsDto;
				response.Success = true;

				_logger.LogInformation("Obteniendo todos los conciertos.");

				return Ok(response);
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Ocurrío un error al obtener la información.";
				_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
				return BadRequest(response);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Post(ConcertRequestDto concertRequestDto)
		{
			var response = new BaseResponseGeneric<int>();
			try
			{
				var genre = await _genreRespository.GetAsync(concertRequestDto.GenreId);
				if (genre is null)
				{
					response.ErrorMessage = $"El género musical con id {concertRequestDto.GenreId} no existe.";
					_logger.LogWarning(response.ErrorMessage);
					
					return BadRequest(response);
				}

				var concertDb = new Concert()
				{
					Title = concertRequestDto.Title,
					Description = concertRequestDto.Description,
					Place = concertRequestDto.Place,
					UnitPrice = concertRequestDto.UnitPrice,
					GenreId = concertRequestDto.GenreId,
					DataEvent = concertRequestDto.DataEvent,
					ImageUrl = concertRequestDto.ImageUrl,
					TicketsQuantity = concertRequestDto.TicketsQuantity					
				};
				
				
				response.Data = await _concertRepository.AddAsync(concertDb);
				response.Success = true;
				
				return Ok(response);
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Ocurrío un error al guardar la información.";
				_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
				return BadRequest(response);
			}
		}
	}
}
