using GleamVault.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GleamVault.Services
{
    public class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;
        private readonly string _localImagePath;

        public ImageService()
        {
            _httpClient = new HttpClient();
            _localImagePath = Path.Combine(FileSystem.AppDataDirectory, "images");
            if (!Directory.Exists(_localImagePath))
                Directory.CreateDirectory(_localImagePath);
        }

        public async Task<string> GetLocalImagePathAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return null;

            var fileName = $"{imageUrl.GetHashCode():X8}_{Path.GetFileName(new Uri(imageUrl).LocalPath)}";
            var localPath = Path.Combine(_localImagePath, fileName);

            if (File.Exists(localPath))
                return localPath;

            try
            {
                var imageBytes = await _httpClient.GetByteArrayAsync(imageUrl);
                await File.WriteAllBytesAsync(localPath, imageBytes);
                return localPath;
            }
            catch
            {
                return imageUrl;
            }
        }

        public Task DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return Task.CompletedTask;

            var fileName = $"{imageUrl.GetHashCode():X8}_{Path.GetFileName(new Uri(imageUrl).LocalPath)}";
            var localPath = Path.Combine(_localImagePath, fileName);

            if (File.Exists(localPath))
                File.Delete(localPath);

            return Task.CompletedTask;
        }

        public Task ClearCacheAsync()
        {
            if (Directory.Exists(_localImagePath))
                Directory.Delete(_localImagePath, true);
            Directory.CreateDirectory(_localImagePath);
            return Task.CompletedTask;
        }

        
    }
}
