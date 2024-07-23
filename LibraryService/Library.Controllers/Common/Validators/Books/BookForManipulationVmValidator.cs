using FluentValidation;
using Library.Controllers.ViewModels;

namespace Library.Controllers.Common.Validators.Books
{
    public abstract class BookForManipulationVmValidator<T> :
        AbstractValidator<T> where T : BookForManipulationViewModel
    {
        protected const string ISBN_REGEX = @"(^(ISBN|ISBN(-|\s)?10)?:?\s*\d{10}$|(^(ISBN|ISBN(-|\s)?13)?:?\s*\d{13}$))";
        protected const int MIN_TITLE_LENGTH = 1;
        protected const int MAX_TITLE_LENGTH = 40;
        protected const int MIN_GENRE_LENGTH = 1;
        protected const int MAX_GENRE_LENGTH = 30;
        protected const int MIN_DESCRIPTION_LENGTH = 1;
        protected const int MAX_DESCRIPTION_LENGTH = 30;
        public BookForManipulationVmValidator()
        {
            RuleFor(b => b.ISBN)
                .NotEmpty()
                .Matches(@"(^(ISBN|ISBN(-|\s)?10)?:?\s*\d{10}$|(^(ISBN|ISBN(-|\s)?13)?:?\s*\d{13}$))");

            RuleFor(b => b.Title)
                .NotEmpty()
                .Length(MIN_TITLE_LENGTH, MAX_TITLE_LENGTH);

            RuleFor(b => b.AuthorId)
                .NotEmpty();

            RuleFor(b => b.Genre)
                .NotEmpty()
                .Length(MIN_GENRE_LENGTH, MAX_GENRE_LENGTH);

            RuleFor(b => b.Description)
                .NotEmpty()
                .Length(MIN_DESCRIPTION_LENGTH, MAX_DESCRIPTION_LENGTH);


            RuleFor(b => b.Image)
                .Must(i => i!.ContentType.StartsWith("image"))
                .WithMessage("File must be image type.")
                .When(b => b.Image != null);
        }
    }

}