using MusicStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Repositories
{
	public interface ISaleRepository : IRepositoryBase<Sale>
	{
		Task CreateTransactionAsync();
		Task RollbackTransactionAsync();
	}
}
