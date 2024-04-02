namespace LibraryApi.Identity.Domain.Core.ConfigModels
{
   public class JwtOptions
   {

      public static string SectionName { get; } = "JwtSettings";

      
      public string? ValidIssuer { get; set; }
      public string? ValidAudience { get; set; }
      public string? Expires { get; set; }

   }
}