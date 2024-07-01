using Microsoft.AspNetCore.Http;

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

		double totalRecords = query.Count();
		httpContext.Response.Headers.Add("TotalRecordsQuantity", totalRecords.ToString());
	}
}
