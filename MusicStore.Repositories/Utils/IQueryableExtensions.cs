using MusicStore.Dto.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Repositories.Utils;

public static class IQueryableExtensions
{
	public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PaginationDto paginationDto)
	{
		return query
			.Skip((paginationDto.Page - 1) * paginationDto.RecordsPerPage)
			.Take(paginationDto.RecordsPerPage);
	}
}
