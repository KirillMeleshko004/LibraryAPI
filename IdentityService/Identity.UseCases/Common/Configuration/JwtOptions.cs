namespace Identity.UseCases.Common.Configuration
{
   //Options class to represent JWTOptions configuration segment
   public class JwtOptions
   {
      public static string SectionName { get; } = "JwtSettings";
      public static string SECRET_ENV { get; } = "LIBRARY_SECRET";


      public string? ValidIssuer { get; set; }
      public string? ValidAudience { get; set; }
      public string? Expires { get; set; }


   }
}