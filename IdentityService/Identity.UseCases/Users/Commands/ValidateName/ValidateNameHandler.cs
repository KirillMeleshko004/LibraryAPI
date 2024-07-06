using Identity.Domain.Entities;
using Identity.UseCases.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.UseCases.Users.Commands
{
    public class ValidateNameHandler : IRequestHandler<ValidateNameCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ValidateNameHandler> _logger;

        public ValidateNameHandler(UserManager<User> userManager,
            ILogger<ValidateNameHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task Handle(ValidateNameCommand request, CancellationToken cancellationToken)
        {
            var res = !string.IsNullOrEmpty(request.UserName) && await _userManager
                .FindByNameAsync(request.UserName) != null;

            if (!res)
            {
                throw new UnauthorizedException("Invalid username/password pair.");
            }
        }
    }
}