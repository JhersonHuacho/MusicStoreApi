using AutoMapper;
using Microsoft.Extensions.Logging;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Repositories;
using MusicStore.Services.Interfaces;

namespace MusicStore.Services.Implementation;

public class SaleService : ISaleService
{
	private readonly ISaleRepository _saleRepository;
	private readonly IConcertRepository _concertRepository;
	private readonly ICustomerRepository _customerRepository;
	private readonly ILogger<SaleService> _logger;
	private readonly IMapper _mapper;

	public SaleService(ISaleRepository saleRepository, IConcertRepository concertRepository,
		ICustomerRepository customerRepository,
		ILogger<SaleService> logger,
		IMapper mapper)
	{
		_saleRepository = saleRepository;
		_concertRepository = concertRepository;
		_customerRepository = customerRepository;
		_logger = logger;
		_mapper = mapper;
	}
	public async Task<BaseResponseGeneric<int>> AddAsync(string email, SaleRequestDto saleRequestDto)
	{
		var response = new BaseResponseGeneric<int>();
		try
		{
			await _saleRepository.CreateTransactionAsync();
			var entity = _mapper.Map<Sale>(saleRequestDto);

			var customer = await _customerRepository.GetByEmailAsync(email);
			if (customer == null)
			{
				throw new InvalidOperationException($"El cliente con email {email} no existe.");
				// Caso de uso: Si el cliente no existe, se crea
				//customer = new Customer
				//{
				//	Email = saleRequestDto.Email,
				//	FullName = saleRequestDto.FullName
				//};
				//customer.Id = await _customerRepository.AddAsync(customer);
			}

			entity.CustomerId = customer.Id;

			var concert = await _concertRepository.GetAsync(saleRequestDto.ConcertId);
			if (concert is null)
			{				
				throw new Exception("Concierto no encontrado");				
			}
			// Se quiere comprar tickets para un concierto que ya empezó.
			if (DateTime.Today > concert.DateEvent)
			{
				throw new InvalidOperationException($"No se puede comprar tickets para el concierto {concert.Title} porque ya pasó");
			}

			// Se quiere comprar tickets para un concierto que ya finalizó.
			if (concert.Finalized)
			{
				throw new InvalidOperationException($"El concierto con id {saleRequestDto.ConcertId} ya finalizó.");
			}

			entity.Total = entity.Quantity * (decimal)concert.UnitPrice;

			await _saleRepository.AddAsync(entity);

			await _saleRepository.UpdateAsync();

			response.Data = entity.Id;
			response.Success = true;
			_logger.LogInformation($"Se creó correctamente la venta para {email}.");
		}
		catch (Exception ex)
		{
			await _saleRepository.RollbackTransactionAsync();
			response.ErrorMessage = "Error al crear la venta";
			_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");			
		}

		return response;
	}

	public async Task<BaseResponseGeneric<SaleResponseDto>> GetAsync(int id)
	{
		var response = new BaseResponseGeneric<SaleResponseDto>();

		try{
			var sale = await _saleRepository.GetAsync(id);
			response.Data = _mapper.Map<SaleResponseDto>(sale);
			response.Success = response.Data != null;
			
		}
		catch (Exception ex)
		{
			response.ErrorMessage = "Error al obtener la venta";
			_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
		}

		return response;
	}

	public async Task<BaseResponseGeneric<ICollection<SaleResponseDto>>> GetAsync(
		SaleByDateSearchDto saleByDateSearchDto, 
		PaginationDto paginationDto)
	{
		var response = new BaseResponseGeneric<ICollection<SaleResponseDto>>();
		try
		{
			var dateInit = Convert.ToDateTime(saleByDateSearchDto.DateStart);
			var dateEnd = Convert.ToDateTime(saleByDateSearchDto.DateEnd);

			var data = await _saleRepository.GetAsync(
					predicate: x => x.SaleDate >= dateInit && x.SaleDate <= dateEnd,
					orderBy: x => x.OperationNumber,
					paginationDto
				);
			
			response.Data = _mapper.Map<ICollection<SaleResponseDto>>(data);
			response.Success = true;
		}
		catch (Exception ex)
		{
			response.ErrorMessage = "Error al obtener las ventas";
			_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");			
		}

		return response;
	}

	public async Task<BaseResponseGeneric<ICollection<SaleResponseDto>>> GetAsync(string email, string title, 
		PaginationDto paginationDto)
	{
		var response = new BaseResponseGeneric<ICollection<SaleResponseDto>>();
		try
		{
			var data = await _saleRepository.GetAsync(
					predicate: s => s.Customer.Email == email && s.Concert.Title.Contains(title ?? string.Empty),
					orderBy: x => x.OperationNumber,
					paginationDto
				);

			response.Data = _mapper.Map<ICollection<SaleResponseDto>>(data);
			response.Success = true;
		}
		catch (Exception ex)
		{
			response.ErrorMessage = "Error al obtener las ventas";
			_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
		}

		return response;
	}

	public async Task<BaseResponseGeneric<ICollection<SaleReportResponseDto>>> GetSaleReportAsync(DateTime dateStart, DateTime dateEnd)
	{
		var response = new BaseResponseGeneric<ICollection<SaleReportResponseDto>>();

		try
		{
			var list = await _saleRepository.GetSaleReportAsync(dateStart, dateEnd);
			response.Data = _mapper.Map<ICollection<SaleReportResponseDto>>(list);
			response.Success = true;
		}
		catch (Exception ex)
		{
			response.ErrorMessage = "Error al obtener el reporte de ventas";
			_logger.LogError(ex, $"{response.ErrorMessage} {ex.Message}");
		}
		return response;
	}
}
