using MusicStore.Services.Interfaces;

namespace MusicStore.Api.Endpoints
{
	public static class HomeEndpoints
	{
		public static void MapHomeEndpoints(this IEndpointRouteBuilder endpoints)
		{
			endpoints.MapGet("/api/Home", async (IConcertService concertService, IGenreService genreService) =>
			{
				var concerts = await concertService.GetAsync("", new Dto.Request.PaginationDto()
				{
					Page = 1,
					RecordsPerPage = 10
				});

				var genres = await genreService.GetAsync();

				return Results.Ok(new
				{
					Concerts = concerts.Data,
					Genres = genres.Data,
					Succes = true
				});
			}).WithDescription("Permite mostrar los enedpoints de la pagina principal");
		}
	}
}
