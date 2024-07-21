using System.ComponentModel.DataAnnotations;

namespace Library.UseCases.Books.DTOs
{
    /// <summary>
    /// Base Dto for books send from user
    /// </summary>

    public abstract record BookForManipulationDto
    {
        public string ISBN { get; set; } = null!;
        public string Title { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public string Genre { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? ImageName { get; set; }
        public Stream? Image { get; set; }
    }
}