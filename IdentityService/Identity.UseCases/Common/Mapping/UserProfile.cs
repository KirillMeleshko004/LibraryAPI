using AutoMapper;
using Identity.Domain.Entities;
using Identity.UseCases.Users.Dtos;

namespace Identity.UseCases.Common.Mapping
{
   public class UserProfile : Profile
   {
      public UserProfile()
      {
         CreateMap<UserForCreationDto, User>();
         CreateMap<UserForAuthorizationDto, User>();
      }
   }
}