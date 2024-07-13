using Microsoft.EntityFrameworkCore;
using MusicStore.Entities;
using MusicStore.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.UnitTests
{
	public class GenreRepositoryTests
	{
		[Fact]
		public async Task CheckGenreCreation()
		{
			// Arrange
			var context = await ArrangeDb();

			// Act
			var genre = new Genre { Name = "Rock", Status = true };
			
			await context.Set<Genre>().AddAsync(genre);
			await context.SaveChangesAsync();

			var count = await context.Set<Genre>().CountAsync();
			var expected = 1;

			// Assert
			Assert.Equal(expected, count);
		}

		[Theory]
		[InlineData("cumbia")]
		public async Task CheckSpecificGenreCreationWithName(string name)
		{
			// Arrange
			var context = await ArrangeDb();

			// Act
			var genre = new Genre() { Name = name, Status = true };

			await context.Set<Genre>().AddAsync(genre);
			await context.SaveChangesAsync();

			var genreFromDb = await context.Set<Genre>().FirstOrDefaultAsync(x => x.Name == name);
			
			// Assert
			Assert.NotNull(genreFromDb);
		}

		[Fact]
		public async Task CheckGenreDeletion()
		{
			// Arrange
			var context = await ArrangeDb();
			
			// Act
			// Creating new genre
			var genre = new Genre { Id = 1, Name = "test genre", Status = true };

			await context.Set<Genre>().AddAsync(genre);
			await context.SaveChangesAsync();
			context.ChangeTracker.Clear();
			
			// Deleting the genre
			var genreToDelete = await context.Set<Genre>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == 1);
			if (genreToDelete is not null)
			{
				context.Remove(new Genre { Id = 1 });
				await context.SaveChangesAsync();
			}

			// Get records count to confirm deletion
			var count = await context.Set<Genre>().AsNoTracking().CountAsync();
			var expected = 0;

			// Assert
			Assert.Equal(expected, count);
		}

		private static async Task<ApplicationDbContext> ArrangeDb()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(Guid.NewGuid().ToString())
				.Options;

			var context = new ApplicationDbContext(options);
			await context.Database.EnsureCreatedAsync();
			return context;
		}
	}
}
