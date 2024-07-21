using Library.UseCases.Books.DTOs;

namespace Library.UseCases.Common.Validators.Books
{
    public class BookForUpdateDtoValidator :
        BookForManipulationDtoValidator<BookForUpdateDto>;
}