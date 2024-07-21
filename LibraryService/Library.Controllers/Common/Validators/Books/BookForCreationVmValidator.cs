using Library.Controllers.ViewModels;

namespace Library.Controllers.Common.Validators.Books
{
    public class BookForCreationVmValidator :
        BookForManipulationVmValidator<BookForCreationViewModel>;
}