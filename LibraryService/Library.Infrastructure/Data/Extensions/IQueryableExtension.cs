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

      private static readonly Dictionary<Type, string> _defaultOrderField = new()
      {
         {typeof(Book), nameof(Book.Title)},
         {typeof(Author), nameof(Author.FirstName)},
         {typeof(Reader), nameof(Reader.Email)},
      };

      public static IQueryable<T> Page<T>(this IQueryable<T> entities,
         RequestParameters parameters)
      {
         return entities
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize);
      }


      public static IQueryable<T> Sort<T>(this IQueryable<T> query,
         string? queryString)
      {
         var orderQuery = CreateOrderQuery<T>(queryString);

         if (string.IsNullOrWhiteSpace(queryString) || string.IsNullOrWhiteSpace(orderQuery))
         {
            var defaultOrderField = _defaultOrderField[typeof(T)];

            return query.OrderBy(defaultOrderField);
         }

         return query.OrderBy(orderQuery);
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