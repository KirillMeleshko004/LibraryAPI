using FluentValidation;
using Library.UseCases.Common.RequestFeatures;

namespace Library.UseCases.Common.Validators.RequestParams
{
    public class BookParametersValidator : RequestParametersValidator<BookParameters>
    {
        protected const int MIN_GENRE_LENGTH = 1;
        protected const int MAX_GENRE_LENGTH = 30;
        protected const int MIN_AUTHOR_LENGTH = 1;
        protected const int MAX_AUTHOR_LENGTH = 20;
        public BookParametersValidator()
        {
            RuleForEach(bp => bp.Genres)
                .Length(MIN_GENRE_LENGTH, MAX_GENRE_LENGTH);

            RuleForEach(bp => bp.Authors)
                .Length(MIN_AUTHOR_LENGTH, MAX_AUTHOR_LENGTH);
        }
    }
}