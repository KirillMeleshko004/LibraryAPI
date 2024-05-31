using AutoMapper;
using Library.Domain.Entities;
using Library.UseCases.Authors.DTOs;

namespace Identity.UseCases.Common.Mapping
{

   public class AuthorProfile : Profile
   {
      public AuthorProfile()
      {

         CreateMap<Author, AuthorDto>().ReverseMap();
         CreateMap<AuthorForCreationDto, Author>();
         CreateMap<AuthorForUpdateDto, Author>();

      }
   }

}