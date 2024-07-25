using AutoMapper;
using Library.Domain.Entities;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Common.Utility;

namespace Library.UseCases.Common.Mapping
{
    public class PagedEnumerable : Profile
    {
        public PagedEnumerable()
        {
            CreateMap(typeof(IPagedEnumerable<>), typeof(IPagedEnumerable<>))
                .ConvertUsing(typeof(PagedEnumerableConverter<,>));
        }

        class PagedEnumerableConverter<TSource, TDest> :
            ITypeConverter<IPagedEnumerable<TSource>, IPagedEnumerable<TDest>>
        {
            public IPagedEnumerable<TDest> Convert(IPagedEnumerable<TSource> source,
                IPagedEnumerable<TDest> destination, ResolutionContext context)
            {
                var itmes = source.Items;
                var metadata = source.Pages;

                var destItems = context.Mapper.Map<IEnumerable<TDest>>(itmes);

                return new PagedList<TDest>(destItems, metadata);
            }
        }
    }
}