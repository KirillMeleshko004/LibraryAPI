namespace LibraryApi.Identity.Shared.Dtos
{
   public record TokenDto
   {

      public string AccessToken { get; set; } = null!;

      public string RefreshToken { get; set; } = null!;

   }
}