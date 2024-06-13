namespace MusicStore.Entities
{
	public class Genre
	{
        public int Id { get; set; }
		// default! hace que el compilador no muestre advertencias de que Name no puede ser nulo y el valor que me asigna es string.Empty
		// y ! hace que el compilador no muestre advertencias de que Name no puede ser nulo y siempre tendrá un valor
		public string Name { get; set; } = default!; //string.Empty;
		public bool Status { get; set; } = true;
    }
}
