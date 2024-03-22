using AutoMapper;
using LibraryAPI.LibraryService.Domain.Core.Entities;
using LibraryAPI.LibraryService.Shared.DTOs;

namespace LibraryAPI.LibraryService.Web.Mapping
{

    public class BookProfile : Profile
    {
        public BookProfile()
        {

            CreateMap<Book, BookDto>();
            CreateMap<BookForCreationDto, Book>();
            CreateMap<BookForUpdateDto, Book>();

        }
    }

}