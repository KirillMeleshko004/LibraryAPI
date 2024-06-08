namespace Library.Infrastructure.Images
{
   public class ImageOptions
   {
      public static string SectionName { get; } = "ImageOptions";

      public string StorePath { get; set; } = null!;
   }
}