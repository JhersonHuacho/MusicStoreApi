using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Castle.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MusicStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStore.Services.Implementation
{
	public class FileStorageAzure : IFileStorage
	{
		private readonly string _azureConnectionString;
		private readonly ILogger<FileStorageAzure> _logger;

		public FileStorageAzure(ILogger<FileStorageAzure> logger, IConfiguration configuration)
		{
			_azureConnectionString = configuration.GetConnectionString("AzureStorage") ?? string.Empty;
			_logger = logger;
		}
		public async Task<string> SaveFile(byte[] content, string extension, string containerName, string contentType)
		{
			string blobUri = string.Empty;

			try
			{
				var client = new BlobContainerClient(_azureConnectionString, containerName);
				await client.CreateIfNotExistsAsync();
				client.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);

				var fileName = $"{Guid.NewGuid()}{extension}";
				var blob = client.GetBlobClient(fileName);

				var blobUploadOptions = new BlobUploadOptions();
				var blobHttpHeader = new BlobHttpHeaders();
				blobHttpHeader.ContentType = contentType;
				blobUploadOptions.HttpHeaders = blobHttpHeader;

				await blob.UploadAsync(new BinaryData(content), blobUploadOptions);
				blobUri = blob.Uri.ToString();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
			}

			return blobUri;
		}

		public async Task<string> EditFile(byte[] content, string extension, string containerName, string path , string contentType)
		{
			await DeleteFile(path, containerName);
			return await SaveFile(content, extension, containerName, contentType);
		}

		public async Task DeleteFile(string path, string containerName)
		{
			try
			{
				if (string.IsNullOrEmpty(path))
				{
					return;
				}

				var client = new BlobContainerClient(_azureConnectionString, containerName);
				await client.CreateIfNotExistsAsync();

				var file = Path.GetFileName(path);
				var blob = client.GetBlobClient(file);

				await blob.DeleteIfExistsAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
			}
		}	
		
	}
}
