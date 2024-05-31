using AutoMapper;
using Identity.Domain.Entities;
using Identity.UseCases.Common.Configuration;
using Identity.UseCases.Users.Dtos;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Identity.UseCases.Users.Commands
{
   public class CreateUserHandler :
      IRequestHandler<CreateUserCommand, (IdentityResult, UserDto?)>
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

      public async Task<(IdentityResult, UserDto?)> Handle(CreateUserCommand request, 
         CancellationToken cancellationToken)
      {
         var user = _mapper.Map<User>(request.UserDto);

         if(string.IsNullOrWhiteSpace(user.UserName))
         {
            user.UserName = user.Email;
         }
         
         var res = await _userManager.CreateAsync(user, request.UserDto.Password);

         if(res.Succeeded)
         {
            await _userManager.AddToRolesAsync(user, request.UserDto.UserRoles);
         }

         var userToReturn = _mapper.Map<UserDto>(user);

         return (res, userToReturn);
      }
   }
}