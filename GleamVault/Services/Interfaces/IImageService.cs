using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GleamVault.Services.Interfaces
{
    public interface IImageService
    {
        Task<string> GetLocalImagePathAsync(string imageUrl);
        Task DeleteImageAsync(string imageUrl);
        Task ClearCacheAsync();
    }
}
