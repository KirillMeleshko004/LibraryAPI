namespace LibraryAPI.LibraryService.Shared.DTOs
{
    /// <summary>
    /// Book DTO that should be send to client
    /// </summary>
    public record BookDto
    {
        public Guid Id { get; set; }
        public string ISBN { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;

        public string? Genre { get; set; }
        public string? Description { get; set; }
        
        public DateTime? BorrowTime { get; set; }
        public DateTime? ReturnTime { get; set; }
    }
}