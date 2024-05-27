using AutoMapper;
using Library.Domain.Entities;
using Library.UseCases.Books.DTOs;

namespace Library.UseCases.Common.Mapping
{

    public class BookProfile : Profile
    {
        public BookProfile()
        {

            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<BookForCreationDto, Book>();
            CreateMap<BookForUpdateDto, Book>();

        }
    }

}