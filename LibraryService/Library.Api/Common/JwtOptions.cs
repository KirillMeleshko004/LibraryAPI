namespace Library.Api.Common
{
   //Options class to represent JWTOptions configuration segment
   public class JwtOptions
   {
      public static string SectionName { get; } = "JwtSettings";


      public string? ValidIssuer { get; set; }
      public string? ValidAudience { get; set; }
      public string? Expires { get; set; }

   }
}