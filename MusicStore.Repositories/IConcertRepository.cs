﻿using MusicStore.Dto.Request;
using MusicStore.Entities;
using MusicStore.Entities.Info;

namespace MusicStore.Repositories
{
	public interface IConcertRepository : IRepositoryBase<Concert>
	{
		Task<ICollection<ConcertInfo>> GetAsync(string? title, PaginationDto paginationDto);
		Task<ICollection<ConcertInfo>> GetLazingAsync(string? title);
		Task<ICollection<ConcertInfo>> GetWithStoredAsync(string? title);
		Task FinalizedAsync(int id);
	}
}
