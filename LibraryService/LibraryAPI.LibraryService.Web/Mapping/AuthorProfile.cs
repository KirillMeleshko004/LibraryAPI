using AutoMapper;
using LibraryAPI.LibraryService.Domain.Core.Entities;
using LibraryAPI.LibraryService.Shared.DTOs;

namespace LibraryAPI.LibraryService.Web.Mapping
{

   public class AuthorProfile : Profile
   {
      public AuthorProfile()
      {

         CreateMap<Author, AuthorDto>();
         CreateMap<AuthorForCreationDto, Author>();
         CreateMap<AuthorForUpdateDto, Author>();

      }
   }

}