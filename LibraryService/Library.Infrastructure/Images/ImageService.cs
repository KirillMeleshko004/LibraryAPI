using System.Security.Cryptography;
using Library.UseCases.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Library.Infrastructure.Images
{
   public class ImageService : IImageService
   {
      private readonly ILogger<ImageService> _logger;
      private readonly ImageOptions _options;

      public ImageService(IOptions<ImageOptions> options, ILogger<ImageService> logger)
      {
         _options = options.Value;
         _logger = logger;
      }

      public async Task<string> SaveImageAsync(Stream image, string imageName)
      {
         var uniqueName = GenerateFileName(imageName);

         var fullPath = Path.Combine(_options.StorePath, uniqueName);

         using (Stream fs = new FileStream(fullPath, FileMode.CreateNew))
         {
            await image.CopyToAsync(fs);
         }

         _logger.LogInformation("File {name} was created.", imageName);

         return Path.Combine(_options.StorePath, uniqueName);
      }

      public async Task DeleteImageAsync(string imagePath)
      {
         if (File.Exists(imagePath))
         {
            File.Delete(imagePath);
            _logger.LogInformation("File {name} was deleted.", imagePath);
         }
         else
         {
            throw new FileNotFoundException(
               $"File with name: {Path.GetFileName(imagePath)} was not found at: {_options.StorePath}");
         }

         await Task.CompletedTask;
      }

      private string GenerateFileName(string fileName)
      {
         var time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
         var guid = Guid.NewGuid();

         return Path.GetFileNameWithoutExtension(fileName) + "_" + time
            + "_" + guid.ToString() + Path.GetExtension(fileName);
      }

   }
}