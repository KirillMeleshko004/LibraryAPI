using System.ComponentModel.DataAnnotations;

namespace Library.UseCases.Authors.DTOs
{
   /// <summary>
   /// Base Dto for authors send from user
   /// </summary>
   public abstract record AuthorForManipulationDto
   {
      public string FirstName { get; set; } = null!;
      public string LastName { get; set; } = null!;
      public DateOnly? DateOfBirth { get; set; }
      public string Country { get; set; } = null!;

   }
}