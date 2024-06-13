using MusicStore.Entities;

namespace MusicStore.Repositories
{
    public class GenreRespository
    {
        private readonly List<Genre> _genreList;

        public GenreRespository()
        {
            _genreList = new List<Genre>();
            _genreList.Add(new Genre { Id = 1, Name = "Rock" });
            _genreList.Add(new Genre { Id = 2, Name = "Salsa" });
            _genreList.Add(new Genre { Id = 3, Name = "Cumbia" });
        }

        public IEnumerable<Genre> Get()
        {
            return _genreList;
        }

        public Genre? Get(int id)
        {
            return _genreList.FirstOrDefault(x => x.Id == id);
        }

        public void Add(Genre genre)
        {
            var lastItem = _genreList.MaxBy(x => x.Id);
            genre.Id = lastItem is null ? 1 : lastItem.Id + 1;
            _genreList.Add(genre);
        }

        public void Update(int id, Genre genre)
        {
            var item = Get(id);

            if (item is not null)
            {
                item.Name = genre.Name;
                item.Status = genre.Status;
            }
        }

        public void Delete(int id)
        {
            var item = Get(id);

            if (item is not null)
            {
                _genreList.Remove(item);
            }
        }
    }
}
