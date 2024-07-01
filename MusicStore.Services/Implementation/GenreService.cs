using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories;
using MusicStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Services.Implementation
{
	public class GenreService : IGenreService
	{
		private readonly IGenreRespository _genreRespository;
		private readonly ILogger<ConcertService> _logger;
		private readonly IMapper _mapper;

		public GenreService(IGenreRespository genreRespository, ILogger<ConcertService> logger, IMapper mapper)
		{
			_genreRespository = genreRespository;
			_logger = logger;
			_mapper = mapper;
		}
		public async Task<BaseResponseGeneric<ICollection<GenreResponseDto>>> GetAsync()
		{
			var response = new BaseResponseGeneric<ICollection<GenreResponseDto>>();
			try
			{
				var data = await _genreRespository.GetAsync();
				response.Data = _mapper.Map<ICollection<GenreResponseDto>>(data);
				response.Success = true;
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Ocurrió un error al obtener la información";
				_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
			}

			return response;
		}

		public async Task<BaseResponseGeneric<GenreResponseDto>> GetAsync(int id)
		{
			var response = new BaseResponseGeneric<GenreResponseDto>();
			try
			{
				var data = await _genreRespository.GetAsync(id);
				response.Data = _mapper.Map<GenreResponseDto>(data);
				response.Success = response.Data != null;
			}
			catch (Exception ex)
			{
				response.ErrorMessage = "Ocurrió un error al obtener la información";
				_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
			}

			return response;
		}

		public async Task<BaseResponseGeneric<int>> AddAsync(GenreRequestDto genreResponseDto)
		{
			var response = new BaseResponseGeneric<int>();
			try
			{
				var genreEntity = _mapper.Map<Genre>(genreResponseDto);
				var data = await _genreRespository.AddAsync(genreEntity);
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

		public async Task<BaseResponse> UpdateAsync(int id, GenreRequestDto genreResponseDto)
		{
			var response = new BaseResponse();
			try
			{
				var data = await _genreRespository.GetAsync(id);
				if (data is null) 
				{
					response.ErrorMessage = $"No existe el genere con id {id}";
				}
				_mapper.Map(genreResponseDto, data);
				await _genreRespository.UpdateAsync();

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
				var data = await _genreRespository.GetAsync(id);
				if (data is null)
				{
					response.ErrorMessage = $"No existe el genere con id {id}";
				}
				
				await _genreRespository.DeleteAsync(id);

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
