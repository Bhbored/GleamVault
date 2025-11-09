using GleamVault.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GleamVault.Services
{
    public class CleanupService
    {
        private readonly IImageService _imageService;

        public CleanupService(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task DeleteProductImageAsync(string imageUrl)
        {
            await _imageService.DeleteImageAsync(imageUrl);
        }

        public async Task DeleteMultipleProductImagesAsync(IEnumerable<string> imageUrls)
        {
            foreach (var url in imageUrls)
            {
                await _imageService.DeleteImageAsync(url);
            }
        }
    }
}
