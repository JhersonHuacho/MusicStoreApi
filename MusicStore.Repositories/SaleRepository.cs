using Microsoft.EntityFrameworkCore;
using MusicStore.Entities;
using MusicStore.Persistence;
using System.Data;

namespace MusicStore.Repositories;

public class SaleRepository : RepositoryBase<Sale>, ISaleRepository
{
	public SaleRepository(ApplicationDbContext context) : base(context)
	{
	}

	public async Task CreateTransactionAsync()
	{
		await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
	}

	public async Task RollbackTransactionAsync()
	{
		await _context.Database.RollbackTransactionAsync();
	}

	public override async Task<int> AddAsync(Sale entity)
	{
		entity.SaleDate = DateTime.Now;
		var nextNumber = await _context.Set<Sale>().CountAsync() + 1;
		entity.OperationNumber = $"{nextNumber:000000}";

		await _context.AddAsync(entity);

		return entity.Id;
	}

	public override async Task UpdateAsync()
	{
		await _context.Database.CommitTransactionAsync();
		await _context.SaveChangesAsync();
	}

	public override async Task<Sale?> GetAsync(int id)
	{
		return await _context.Set<Sale>()
			.Include(s => s.Customer)
			.Include(s => s.Concert)
			.ThenInclude(c => c.Genre)
			.Where(s => s.Id == id)
			.AsNoTracking()
			.IgnoreQueryFilters()
			.FirstOrDefaultAsync();
	}
}
