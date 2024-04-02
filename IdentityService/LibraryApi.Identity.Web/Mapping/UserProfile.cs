using AutoMapper;
using LibraryApi.Identity.Domain.Core.Entities;
using LibraryApi.Identity.Shared.Dtos;

namespace LibraryApi.Identity.Web.Mapping
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