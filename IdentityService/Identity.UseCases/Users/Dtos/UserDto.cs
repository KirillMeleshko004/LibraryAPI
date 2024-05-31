namespace Identity.UseCases.Users.Dtos
{
   public record UserDto
   {
      public Guid Id { get; set; }
      public string Email { get; set; } = null!;
      public string FirstName { get; set; } = null!;
      public string LastName { get; set; } = null!;
      public  List<string> Roles { get; set; } = [];
   }
}