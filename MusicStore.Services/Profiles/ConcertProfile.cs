using AutoMapper;
using MusicStore.Dto.Request;
using MusicStore.Dto.Response;
using MusicStore.Entities;
using MusicStore.Entities.Info;

namespace MusicStore.Services.Profiles
{
	public class ConcertProfile : Profile
	{
        public ConcertProfile()
        {
            CreateMap<ConcertInfo, ConcertResponseDto>();
            CreateMap<Concert, ConcertResponseDto>()
                .ForMember(dest => dest.DateEvent, opt => opt.MapFrom(src => src.DateEvent.ToShortDateString()))
				.ForMember(dest => dest.TimeEvent, opt => opt.MapFrom(src => src.DateEvent.ToShortTimeString()))
				.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status ? "Activo" : "Inactivo"))
                ;
            CreateMap<ConcertResponseDto, Concert>()
                .ForMember(dest => dest.DateEvent, opt => opt.MapFrom(src => Convert.ToDateTime($"{src.DateEvent} {src.TimeEvent}")))
				.ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                ;

			CreateMap<ConcertRequestDto, Concert>()
				.ForMember(dest => dest.DateEvent, opt => opt.MapFrom(src => Convert.ToDateTime($"{src.DateEvent} {src.TimeEvent}")))
				.ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
        }
    }
}
