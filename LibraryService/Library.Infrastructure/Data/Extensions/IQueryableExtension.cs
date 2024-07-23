using Library.Domain.Entities;
using Library.UseCases.Common.RequestFeatures;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Library.Infrastructure.Data.Extensions
{
   /// <summary>
   /// Class with extension helper methods for IQueryable<T>
   /// </summary>
   public static class IQueryableExtension
   {
      public static IQueryable<Book> FilterBooks(this IQueryable<Book> books,
         BookParameters parameters)
      {
         if (parameters.Authors != null && parameters.Authors.Length > 0)
         {
            books = books.Where(b => parameters.Authors.Contains(b.AuthorName));
         }

         if (parameters.Genres != null && parameters.Genres.Length > 0)
         {
            books = books.Where(b => parameters.Genres.Contains(b.Genre));
         }

         return books;
      }

      public static IQueryable<Author> FilterAuthors(this IQueryable<Author> authors,
         AuthorParameters parameters)
      {
         return authors;
      }

      public static IQueryable<T> Page<T>(this IQueryable<T> entities,
         RequestParameters parameters)
      {
         return entities
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize);
      }

      public static IQueryable<Book> Sort(this IQueryable<Book> books,
         string? queryString)
      {
         var orderQuery = CreateOrderQuery<Book>(queryString);

         if (string.IsNullOrWhiteSpace(queryString) || string.IsNullOrWhiteSpace(orderQuery))
            return books.OrderBy(b => b.Title);

         return books.OrderBy(orderQuery);
      }

      public static IQueryable<Author> Sort(this IQueryable<Author> books,
         string? queryString)
      {
         var orderQuery = CreateOrderQuery<Author>(queryString);

         if (string.IsNullOrWhiteSpace(queryString) || string.IsNullOrWhiteSpace(orderQuery))
            return books.OrderBy(b => b.FirstName);

         return books.OrderBy(orderQuery);
      }

      public static IQueryable<Book> SearchByName(this IQueryable<Book> books,
         string? searchTerm)
      {
         if (string.IsNullOrWhiteSpace(searchTerm)) return books;

         //Convert toLower there since ef core can not translate 
         //string.Contains with StringComparison to query
         var lowerTerm = searchTerm.Trim().ToLower();

#pragma warning disable CA1862 // Use the 'StringComparison' method overloads to perform case-insensitive string comparisons

         return books.Where(b => b.Title.ToLower()
            .Contains(lowerTerm));

#pragma warning restore CA1862 // Use the 'StringComparison' method overloads to perform case-insensitive string comparisons

      }

      #region Private methods

      private static string CreateOrderQuery<T>(string? queryString)
      {
         if (string.IsNullOrWhiteSpace(queryString)) return string.Empty;

         var result = new StringBuilder();

         //Select all non static public properies
         var properties = typeof(T)
               .GetProperties(BindingFlags.Public | BindingFlags.Instance);

         //retrieve all tokens from query string
         var tokens = queryString.Trim().Split(',');

         foreach (var token in tokens)
         {
            if (string.IsNullOrWhiteSpace(token)) continue;

            var prop = token.Split(' ').FirstOrDefault();

            //Searching property with passed name inside object
            var propName = properties
               .Where(p => p.Name.Equals(prop, StringComparison.InvariantCultureIgnoreCase))
               .Select(p => p.Name)
               .FirstOrDefault();

            if (propName == null) continue;

            var direction = token.EndsWith("desc") ? "descending" : "ascending";

            result.Append($"{propName} {direction},");
         }

         if (result.Length == 0) return string.Empty;

         var orderString = result.ToString().TrimEnd(',', ' ');

         return orderString;
      }

      #endregion

   }
}