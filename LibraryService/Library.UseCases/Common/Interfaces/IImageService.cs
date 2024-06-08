namespace Library.UseCases.Common.Interfaces
{
   public interface IImageService
   {
      Task<string> SaveImageAsync(Stream image, string imageName);
      Task DeleteImageAsync(string imagePath);
   }
}