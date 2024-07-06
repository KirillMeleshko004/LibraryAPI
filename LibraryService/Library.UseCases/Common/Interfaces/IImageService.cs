namespace Library.UseCases.Common.Interfaces
{
   /// <summary>
   /// Service for image manipulations
   /// </summary>
   public interface IImageService
   {
      /// <summary>
      /// Save image
      /// </summary>
      /// <param name="image">Stream with image</param>
      /// <param name="imageName">Name of image with extension</param>
      /// <returns>Path to saved image</returns>
      Task<string> SaveImageAsync(Stream image, string imageName);

      /// <summary>
      /// Delete specified image 
      /// </summary>
      /// <param name="imagePath">Path to image</param>
      Task DeleteImageAsync(string imagePath);
   }
}