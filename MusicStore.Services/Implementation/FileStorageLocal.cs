using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MusicStore.Services.Interfaces;

namespace MusicStore.Services.Implementation
{
	public class FileStorageLocal : IFileStorage
	{
		private readonly ILogger<FileStorageLocal> _logger;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public FileStorageLocal(ILogger<FileStorageLocal> logger, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
		{
			_logger = logger;
			_webHostEnvironment = webHostEnvironment;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<string> SaveFile(byte[] content, string extension, string containerName, string contentType)
		{
			string databaseUrl = string.Empty;
			try
			{
				var fileName = $"{Guid.NewGuid()}{extension}";
				string folder = Path.Combine(_webHostEnvironment.WebRootPath, containerName);

				if (!Directory.Exists(folder))
				{
					Directory.CreateDirectory(folder);
				}

				string path = Path.Combine(folder, fileName);
				await File.WriteAllBytesAsync(path, content);

				var currentUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
				databaseUrl = Path.Combine(currentUrl, containerName, fileName).Replace("\\", "/");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
			}

			return databaseUrl;
		}

		public async Task<string> EditFile(byte[] content, string extension, string containerName, string path, string contentType)
		{
			await DeleteFile(path, containerName);
			return await SaveFile(content, extension, containerName, contentType);
		}

		public Task DeleteFile(string path, string containerName)
		{
			try
			{
				if (path is not null)
				{
					string fileName = Path.GetFileName(path);
					string fileDirectory = Path.Combine(_webHostEnvironment.WebRootPath, containerName, fileName);

					if (File.Exists(fileDirectory))
					{
						File.Delete(fileDirectory);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
			}

			return Task.FromResult(0);
		}			
	}
}
