using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Services.Interfaces
{
	public interface IFileStorage
	{
		Task<string> SaveFile(byte[] content, string extension, string containerName, string contentType);
		Task<string> EditFile(byte[] content, string extension, string containerName, string path, string contentType);
		Task DeleteFile(string path, string containerName);
	}
}
