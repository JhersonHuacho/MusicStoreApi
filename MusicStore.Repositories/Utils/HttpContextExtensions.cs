using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MusicStore.Repositories.Utils;

public static class HttpContextExtensions
{
	public async static Task InsertPaginationHeader<T>(this HttpContext httpContext,
		IQueryable<T> query)
	{
		if (httpContext == null)
		{
			throw new ArgumentNullException(nameof(httpContext));
		}

		double totalRecords = await query.CountAsync();
		httpContext.Response.Headers.Add("TotalRecordsQuantity", totalRecords.ToString());
	}
}
