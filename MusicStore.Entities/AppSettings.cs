using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Entities
{
	public class AppSettings
	{
		public Jwt Jwt { get; set; }
	}

	public class Jwt
	{
		public string JWTKey { get; set; } = default!;
		public int LifetimeInSeconds { get; set; }
	}
}
