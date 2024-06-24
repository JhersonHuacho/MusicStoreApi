namespace MusicStore.Entities
{
	public class Concert : EntityBase
	{ 
		public string Title { get; set; } = default!;
		public string Description { get; set; } = default!;
		public string Place { get; set; } = default!;
		public double UnitPrice { get; set; }
		public int GenreId { get; set; }
		public DateTime DataEvent { get; set; }
		public string? ImageUrl { get; set; }
		public int TicketsQuantity { get; set; }
		public bool Finalized { get; set; }		
		// Navigation properties es una propiedad de navegación que permite a EF Core saber que hay una relación entre dos entidades
		// quiere decir que hay una relación entre Concert y Genre de uno a muchos (1:N) y que Genre es la entidad principal
		// y Concert es la entidad secundaria 
		// 1 Genre puede tener muchos Concert
		public Genre Genre { get; set; } = default!;
    }
}
