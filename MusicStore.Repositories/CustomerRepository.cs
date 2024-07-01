using Microsoft.EntityFrameworkCore;
using MusicStore.Entities;
using MusicStore.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Repositories;

public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
	public CustomerRepository(ApplicationDbContext context) : base(context)
	{
	}

	public async Task<Customer?> GetByEmailAsync(string email)
	{
		return await _context.Set<Customer>().FirstOrDefaultAsync(c => c.Email == email);
	}
}
