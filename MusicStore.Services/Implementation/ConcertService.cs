using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories;
using MusicStore.Services.Interfaces;

namespace MusicStore.Services.Implementation
{
	public class ConcertService : IConcertService
	{
		private readonly IConcertRepository _concertRepository;
		private readonly ILogger<ConcertService> _logger;
		private readonly IMapper _mapper;

		public ConcertService(IConcertRepository concertRepository, ILogger<ConcertService> logger, IMapper mapper)
        {
			_concertRepository = concertRepository;
			_logger = logger;
			_mapper = mapper;
		}
        public async Task<BaseResponseGeneric<ICollection<ConcertResponseDto>>> GetAsync(string? title)
		{
			var response = new BaseResponseGeneric<ICollection<ConcertResponseDto>>();
			try
			{
				var data = await _concertRepository.GetAsync(title);
				response.Data = _mapper.Map<ICollection<ConcertResponseDto>>(data);
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Ocurrió un error al obtener la información";
				_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}" );
			}

			return response;
		}

		public async Task<BaseResponseGeneric<ConcertResponseDto>> GetAsync(int id)
		{
			var response = new BaseResponseGeneric<ConcertResponseDto>();
			try
			{
				var data = await _concertRepository.GetAsync(id);
				if (data is null)
				{
					response.ErrorMessage = $"El concierto con id {id} no existe.";
					_logger.LogWarning(response.ErrorMessage);
					return response;
				}
				response.Data = _mapper.Map<ConcertResponseDto>(data);
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Ocurrió un error al obtener la información";
				_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
			}

			return response;
		}

		public async Task<BaseResponseGeneric<int>> AddAsync(ConcertRequestDto concert)
		{
			var response = new BaseResponseGeneric<int>();
			try
			{
				var concertEntity = _mapper.Map<Concert>(concert);
				var data = await _concertRepository.AddAsync(concertEntity);
				response.Data = data;
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Ocurrió un error al obtener la información";
				_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
			}

			return response;
		}

		public async Task<BaseResponse> UpdateAsync(int id, ConcertRequestDto concertRequestDto)
		{
			var response = new BaseResponse();
			try
			{
				var data = await _concertRepository.GetAsync(id);
				if (data is null)
				{
					response.ErrorMessage = "No se encontró el registro";
					return response;
				}
				
				_mapper.Map(concertRequestDto, data);
				await _concertRepository.UpdateAsync();
				
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Ocurrió un error al obtener la información";
				_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
			}

			return response;
		}

		public async Task<BaseResponse> DeleteAsync(int id)
		{
			var response = new BaseResponse();
			try
			{
				var data = await _concertRepository.GetAsync(id);
				if (data is null)
				{
					response.ErrorMessage = "No se encontró el registro a eliminar";
					return response;
				}
				
				await _concertRepository.DeleteAsync(id);
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Ocurrió un error al obtener la información";
				_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
			}

			return response;
		}		

		public async Task<BaseResponse> FinalizeAsync(int id)
		{
			var response = new BaseResponse();
			try
			{
				var data = await _concertRepository.GetAsync(id);
				if (data is null)
				{
					response.ErrorMessage = "No se encontró el registro";
					return response;
				}
				
				await _concertRepository.FinalizedAsync(id);
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Ocurrió un error al obtener la información";
				_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
			}

			return response;
		}		

	}
}
