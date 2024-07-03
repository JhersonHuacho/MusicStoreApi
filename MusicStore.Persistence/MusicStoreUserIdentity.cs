using Microsoft.AspNetCore.Identity;

namespace MusicStore.Persistence
{
	public class MusicStoreUserIdentity : IdentityUser
	{
		public string FirstName { get; set; } = default!;
		public string LastName { get; set; } = default!;
		public int Age { get; set; }
		public DocumentTypeEnum DocumentType { get; set; }
		public string DocumentNumber { get; set; } = default!;
	}

	public enum DocumentTypeEnum : short
	{
		Dni = 1,
		Passport = 2
	}
}
