using FluentValidation;
using Identity.Domain.Enum;
using Identity.UseCases.Users.Dtos;

namespace Identity.UseCases.Common.Validators
{
    public class UserForCreationDtoValidator : AbstractValidator<UserForCreationDto>
    {
        protected const int MIN_USERNAME_LENGTH = 4;
        protected const int MAX_USERNAME_LENGTH = 40;
        protected const int MIN_PASSWORD_LENGTH = 10;
        protected const int MAX_PASSWORD_LENGTH = 20;
        protected const int MAX_EMAIL_LENGTH = 40;
        protected const int MIN_NAME_LENGTH = 1;
        protected const int MAX_NAME_LENGTH = 40;
        protected const string NAME_REGEX = @"(?i)^[a-z ,.'-]+$";
        public UserForCreationDtoValidator()
        {
            RuleFor(u => u.UserName).NotEmpty()
                .Length(MIN_USERNAME_LENGTH, MAX_USERNAME_LENGTH);
            RuleFor(u => u.Password).NotEmpty()
                .Length(MIN_PASSWORD_LENGTH, MAX_PASSWORD_LENGTH);

            RuleFor(u => u.FirstName).NotEmpty()
                .Length(MIN_NAME_LENGTH, MAX_NAME_LENGTH)
                .Matches(NAME_REGEX);
            RuleFor(u => u.LastName).NotEmpty()
                .Length(MIN_NAME_LENGTH, MAX_NAME_LENGTH)
                .Matches(NAME_REGEX);

            RuleFor(u => u.Email).NotEmpty()
                .MaximumLength(MAX_EMAIL_LENGTH)
                .EmailAddress();

            RuleFor(u => u.UserRoles).NotEmpty();
            RuleForEach(u => u.UserRoles)
                .NotEmpty()
                .IsEnumName(typeof(Roles), false);
        }
    }
}