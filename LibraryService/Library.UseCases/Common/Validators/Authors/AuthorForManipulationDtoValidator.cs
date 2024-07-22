using FluentValidation;
using Library.UseCases.Authors.DTOs;

namespace Library.UseCases.Common.Validators.Authors
{
    public abstract class AuthorForManipulationDtoValidator<T> : AbstractValidator<T>
        where T : AuthorForManipulationDto
    {
        protected const int MIN_NAME_LENGTH = 1;
        protected const int MAX_NAME_LENGTH = 20;
        protected const int MIN_COUNTRY_LENGTH = 2;
        protected const int MAX_COUNTRY_LENGTH = 73;
        public AuthorForManipulationDtoValidator()
        {
            RuleFor(a => a.FirstName)
                .NotEmpty()
                .Length(MIN_NAME_LENGTH, MAX_NAME_LENGTH);

            RuleFor(a => a.LastName)
                .NotEmpty()
                .Length(MIN_NAME_LENGTH, MAX_NAME_LENGTH);

            RuleFor(a => a.DateOfBirth).NotEmpty()
                .Must(d => d.CompareTo(DateOnly.FromDateTime(DateTime.Now)) < 0);

            RuleFor(a => a.Country)
                .NotEmpty()
                .Length(MIN_COUNTRY_LENGTH, MAX_COUNTRY_LENGTH);

        }
    }
}