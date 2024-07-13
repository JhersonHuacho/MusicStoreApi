using Microsoft.AspNetCore.Authorization;
using MusicStore.Entities;
using MusicStore.Services.Interfaces;

namespace MusicStore.Api.Endpoints
{
	public static class ReportEndpoints
	{
		public static void MapReportEndpoints(this IEndpointRouteBuilder routes)
		{
			var group = routes.MapGroup("api/Reports")
				.WithDescription("Endpoints para generar reportes")
				.WithTags("Reports");

			group.MapGet("/", [Authorize(Roles = Constants.RoleAdmin)] async (ISaleService saleService, string dateStart, string dateEnd) =>
			{
				var response = await saleService.GetSaleReportAsync(DateTime.Parse(dateStart), DateTime.Parse(dateEnd));
				
				return response.Success ? Results.Ok(response) : Results.BadRequest(response);
			});
		}
	}
}
