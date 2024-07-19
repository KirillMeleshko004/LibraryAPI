using FluentValidation;
using Identity.UseCases.Users.Dtos;

namespace Identity.UseCases.Common.Validators
{
    public class UserForAuthorizationDtoValidator : AbstractValidator<UserForAuthorizationDto>
    {
        protected const int MIN_PASSWORD_LENGTH = 10;
        protected const int MAX_PASSWORD_LENGTH = 20;
        protected const int MIN_USERNAME_LENGTH = 4;
        protected const int MAX_USERNAME_LENGTH = 40;
        public UserForAuthorizationDtoValidator()
        {
            RuleFor(u => u.UserName).NotEmpty()
                .Length(MIN_USERNAME_LENGTH, MAX_USERNAME_LENGTH);

            RuleFor(u => u.Password).NotEmpty()
                .Length(MIN_PASSWORD_LENGTH, MAX_PASSWORD_LENGTH);
        }
    }
}