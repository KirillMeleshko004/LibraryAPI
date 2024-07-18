using AutoMapper;
using Library.Api.Controllers.ViewModels;
using Library.UseCases.Books.DTOs;
using Microsoft.AspNetCore.Http;

namespace Library.Controllers.Common.Mapping
{

   public class BookProfile : Profile
   {
      public BookProfile()
      {

         CreateMap<IFormFile?, Stream?>()
            .ConvertUsing<IFormFileTypeConverter>();

         CreateMap<BookForCreationViewModel, BookForCreationDto>()
            .ForMember(dto => dto.ImageName, options =>
            {
               options.Condition(vm => vm.Image != null);
               options.MapFrom(vm => Path.GetFileName(vm.Image!.FileName));
            });
      }
   }

   public class IFormFileTypeConverter : ITypeConverter<IFormFile?, Stream?>
   {
      public Stream? Convert(IFormFile? source, Stream? destination, ResolutionContext context)
      {
         if (source == null) return null;
         var stream = new MemoryStream();
         source.CopyTo(stream);
         stream.Position = 0;
         return stream;
      }
   }

}