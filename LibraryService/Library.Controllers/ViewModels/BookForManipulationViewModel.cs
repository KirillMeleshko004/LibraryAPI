using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Library.Controllers.ViewModels
{
    public abstract record BookForManipulationViewModel
    {
        public string ISBN { get; set; } = null!;
        public string Title { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public string Genre { get; set; } = null!;
        public string Description { get; set; } = null!;
        public IFormFile? Image { get; set; }
    }
}