using MediatR;

namespace Identity.UseCases.Users.Commands
{
    public record ValidateNameCommand(string? UserName) : IRequest
    {
    }
}