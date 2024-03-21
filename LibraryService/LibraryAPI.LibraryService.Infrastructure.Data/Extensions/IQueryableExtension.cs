using LibraryAPI.LibraryService.Domain.Core.Entities;
using LibraryAPI.LibraryService.Shared.RequestFeatures;

namespace LibraryAPI.LibraryService.Infrastructure.Data.Extensions
{
    public static class IQueryableExtension
    {
        public static IQueryable<Book> FilterBooks(this IQueryable<Book> books, 
            BookParameters parameters)
        {
            return books.PageBooks(parameters);
        }

        public static IQueryable<Book> PageBooks(this IQueryable<Book> books, 
            RequestParameters parameters)
        {
            return books
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize);
        }

        public static IQueryable<Book> SortBooks(this IQueryable<Book> books, 
            string? orderByString)
        {
            return books;
        }
    }
}