using FluentValidation;
using Library.UseCases.Common.RequestFeatures;

namespace Library.UseCases.Common.Validators.RequestParams
{
    public class RequestParametersValidator : AbstractValidator<RequestParameters>
    {
        protected const int MIN_PAGE_SIZE = 1;
        protected const int MAX_PAGE_SIZE = 50;
        protected const int ORDER_MAX_LENGTH = 100;

        public RequestParametersValidator()
        {
            RuleFor(p => p.PageNumber)
                .GreaterThan(0);

            RuleFor(p => p.PageSize)
                .InclusiveBetween(MIN_PAGE_SIZE, MAX_PAGE_SIZE);

            RuleFor(p => p.OrderBy)
                .MaximumLength(ORDER_MAX_LENGTH);

        }
    }
}