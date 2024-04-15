namespace LibraryAPI.LibraryService.Shared.DTOs
{
   /// <summary>
   /// Author DTO that should be send to client
   /// </summary>
   public record AuthorDto
   {
      public Guid Id { get; set; }

      public string FirstName { get; set; } = null!;
      public string LastName { get; set; } = null!;
   }
}