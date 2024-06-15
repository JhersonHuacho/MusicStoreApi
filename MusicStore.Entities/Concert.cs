using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Entities
{
	public class Concert
	{
		public int Id { get; set; }
		public string Title { get; set; } = default!;
		public string Description { get; set; } = default!;
		public string Place { get; set; } = default!;
		public double UnitPrice { get; set; }
		public int GenreId { get; set; }
		public DateTime DataEvent { get; set; }
		public string? ImageUrl { get; set; }
		public int TicketsQuantity { get; set; }
		public bool Finalized { get; set; }
		public bool Status { get; set; } = true;
		// Navigation properties es una propiedad de navegación que permite a EF Core saber que hay una relación entre dos entidades
		public Genre Genre { get; set; } = default!;
    }
}
