namespace LibraryAPI.LibraryService.Shared.DTOs
{
   public abstract record AuthorForManipulationDto
   {
      public Guid Id { get; set; }

      public string FirstName { get; set; } = null!;
      public string LastName { get; set; } = null!;
   }
}