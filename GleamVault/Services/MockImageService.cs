using GleamVault.Services.Interfaces;

public class MockImageService : IImageService
{
    private readonly string _mockImagesPath;
    private readonly List<string> _mockImageFiles = new();

    public MockImageService()
    {
        _mockImagesPath = Path.Combine(FileSystem.AppDataDirectory, "mock_images");
        InitializeMockImages();
    }

    private void InitializeMockImages()
    {
        if (!Directory.Exists(_mockImagesPath))
            Directory.CreateDirectory(_mockImagesPath);

        CopyLocalImagesToMockFolder();
        LoadMockImageFiles();
    }

    private void CopyLocalImagesToMockFolder()
    {
        string localImagesPath = @"C:\Users\Bhbored\Documents\C#\Maui\GleamVault\GleamVault\Resources\Images";

        if (Directory.Exists(localImagesPath))
        {
            var imageFiles = Directory.GetFiles(localImagesPath, "*.*")
                .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png") || f.EndsWith(".jpeg"));

            foreach (var sourceFile in imageFiles)
            {
                var fileName = Path.GetFileName(sourceFile);
                var destFile = Path.Combine(_mockImagesPath, fileName);

                if (!File.Exists(destFile))
                {
                    File.Copy(sourceFile, destFile, true);
                }
            }
        }
    }

    private void LoadMockImageFiles()
    {
        if (Directory.Exists(_mockImagesPath))
        {
            var files = Directory.GetFiles(_mockImagesPath, "*.*")
                .Where(f => f.EndsWith(".jpg") || f.EndsWith(".png") || f.EndsWith(".jpeg"))
                .ToList();
            _mockImageFiles.AddRange(files);
        }
    }

    public Task<string> GetLocalImagePathAsync(string imageUrl)
    {
        if (_mockImageFiles.Count == 0)
            return Task.FromResult<string>(null);

        var random = new Random();
        var randomImage = _mockImageFiles[random.Next(_mockImageFiles.Count)];
        return Task.FromResult(randomImage);
    }

   

    public Task DeleteImageAsync(string imageUrl)
    {
        if (!string.IsNullOrEmpty(imageUrl) && File.Exists(imageUrl))
        {
            File.Delete(imageUrl);
            _mockImageFiles.Remove(imageUrl);
        }
        return Task.CompletedTask;
    }

    public Task ClearCacheAsync()
    {
        if (Directory.Exists(_mockImagesPath))
            Directory.Delete(_mockImagesPath, true);

        Directory.CreateDirectory(_mockImagesPath);
        _mockImageFiles.Clear();
        CopyLocalImagesToMockFolder();
        LoadMockImageFiles();
        return Task.CompletedTask;
    }
}