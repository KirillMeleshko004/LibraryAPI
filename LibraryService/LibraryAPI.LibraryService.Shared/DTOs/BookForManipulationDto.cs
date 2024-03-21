namespace LibraryAPI.LibraryService.Shared.DTOs
{
    public abstract record BookForManipulationDto
    {
        public string? ISBN { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Genre { get; set; }
        public string? Description { get; set; }
        
        public DateTime? BorrowTime { get; set; }
        public DateTime? ReturnTime { get; set; }
    }
}