using MusicStore.Entities;
using MusicStore.Persistence;

namespace MusicStore.Repositories
{
	public class GenreRespository : RepositoryBase<Genre>, IGenreRespository
	{
		public GenreRespository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
