using System.Text;
using AutoMapper;
using Identity.Domain.Entities;
using Identity.UseCases.Common.Messages;
using Identity.UseCases.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.UseCases.Users.Commands
{
   /// <summary>
   /// Handler for CreateUserCommand
   /// </summary>
   public class CreateUserHandler :
      IRequestHandler<CreateUserCommand>
   {
      private readonly UserManager<User> _userManager;
      private readonly IMapper _mapper;
      private readonly ILogger<CreateUserHandler> _logger;

      public CreateUserHandler(UserManager<User> userManager, IMapper mapper,
         ILogger<CreateUserHandler> logger)
      {
         _userManager = userManager;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task Handle(CreateUserCommand request,
         CancellationToken cancellationToken)
      {
         var user = _mapper.Map<User>(request.UserDto);

         if (string.IsNullOrWhiteSpace(user.UserName))
         {
            user.UserName = user.Email;
         }

         var res = await _userManager.CreateAsync(user, request.UserDto.Password);

         if (res.Succeeded)
         {
            await _userManager.AddToRolesAsync(user, request.UserDto.UserRoles);
         }
         else
         {
            _logger.LogInformation(LoggingMessages.UserCreationFailedLog,
               res.Errors);

            var message = new StringBuilder();
            foreach (var error in res.Errors)
            {
               message.Append($"{error.Description} ");
            }

            throw new UnprocessableEntityException(message.ToString());
         }
      }
   }
}