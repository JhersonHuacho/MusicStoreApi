using Microsoft.AspNetCore.Mvc;
using MusicStore.Entities;
using MusicStore.Repositories;

namespace MusicStore.Api.Controllers
{
	[ApiController]
	[Route("api/genres")]
	public class GenresController : ControllerBase
	{
		private readonly GenreRespository _genreRespository;

		public GenresController(GenreRespository genreRespository)
        {
			_genreRespository = genreRespository;
		}

		[HttpGet]
		public ActionResult<List<Genre>> Get()
		{
			var data = _genreRespository.Get();
			return Ok(data);
		}

		[HttpGet("{id:int}")]
		public ActionResult<Genre> Get(int id)
		{
			var data = _genreRespository.Get(id);
			return data is null ? NotFound() : Ok(data);
		}

		[HttpPost]
		public ActionResult Post([FromBody] Genre genre)
		{
			_genreRespository.Add(genre);
			return Ok();
		}

		[HttpPut("{id:int}")]
		public ActionResult Put(int id, [FromBody] Genre genre)
		{
			_genreRespository.Update(id, genre);
			return Ok();
		}

		[HttpDelete("{id:int}")]
		public ActionResult Delete(int id)
		{
			_genreRespository.Delete(id);
			return Ok();
		}
	}
}
