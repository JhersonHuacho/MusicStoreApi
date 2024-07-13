using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Dto.Response
{
	public class SaleReportResponseDto
	{
		public string ConcertName { get; set; } = default!;
		public decimal Total { get; set; }
	}
}
