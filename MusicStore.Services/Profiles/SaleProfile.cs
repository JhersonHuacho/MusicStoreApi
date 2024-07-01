using AutoMapper;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using System.Globalization;

namespace MusicStore.Services.Profiles;

public class SaleProfile : Profile
{
	private static readonly CultureInfo culture = new("es-PE");
	public SaleProfile()
	{
		CreateMap<SaleRequestDto, Sale>()
			.ForMember(dest => dest.Quantity, opt => opt.MapFrom(x => x.TicketsQuantity));

		CreateMap<Sale, SaleResponseDto>()
			.ForMember(dest => dest.SaleId, opt => opt.MapFrom(x => x.Id))
			.ForMember(dest => dest.DateEvent, opt => opt.MapFrom(x => x.Concert.DateEvent.ToString("D", culture)))
			.ForMember(dest => dest.TimeEvent, opt => opt.MapFrom(x => x.Concert.DateEvent.ToString("T", culture)))
			.ForMember(dest => dest.Genre, opt => opt.MapFrom(x => x.Concert.Genre.Name))
			.ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(x => x.Concert.ImageUrl))
			.ForMember(dest => dest.Title, opt => opt.MapFrom(x => x.Concert.Title))			
			.ForMember(dest => dest.FullName, opt => opt.MapFrom(x => x.Customer.FullName))			
			.ForMember(dest => dest.SaleDate, opt => opt.MapFrom(x => x.SaleDate.ToString("dd/MM/yyyy HH:mm", culture)))
			;

	}
}
