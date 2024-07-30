using AutoMapper.Internal;
using Library.Domain.Entities;
using Library.UseCases.Common.RequestFeatures;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Library.Infrastructure.Data.Extensions
{
   /// <summary>
   /// Class with extension helper methods for IQueryable<T>
   /// </summary>
   public static class IQueryableExtension
   {

      private static readonly Dictionary<Type, string> _defaultField = new()
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
            var defaultOrderField = _defaultField[typeof(T)];

            return query.OrderBy(defaultOrderField);
         }

         return query.OrderBy(orderQuery);
      }

      public static IQueryable<T> Search<T>(this IQueryable<T> query, string? term)
      {
         if (string.IsNullOrWhiteSpace(term)) return query;

         var lowerTerm = term.Trim().ToLower();

         var expr = BuildSearchExpression<T>(_defaultField[typeof(T)], lowerTerm);

         return query.Where(expr);
      }

      public static IQueryable<T> Filter<T>(this IQueryable<T> query,
         Dictionary<string, IEnumerable<string>>? filters)
      {
         if (filters == null) return query;

         foreach (var filter in filters)
         {
            var propName = GetValidPropName<T>(filter.Key);

            if (propName == null) continue;

            var exp = BuildFilterExpression<T>(propName, filter.Value.AsQueryable());

            query = query.Where(exp);
         }

         return query;
      }

      #region Private methods

      private static string CreateOrderQuery<T>(string? queryString)
      {
         if (string.IsNullOrWhiteSpace(queryString)) return string.Empty;

         var result = new StringBuilder();

         //retrieve all tokens from query string
         var tokens = queryString.Trim().Split(',');

         foreach (var token in tokens)
         {
            if (string.IsNullOrWhiteSpace(token)) continue;

            var prop = token.Split(' ').FirstOrDefault();

            var propName = GetValidPropName<T>(prop);

            if (propName == null) continue;

            var direction = token.EndsWith("desc") ? "descending" : "ascending";

            result.Append($"{propName} {direction},");
         }

         if (result.Length == 0) return string.Empty;

         var orderString = result.ToString().TrimEnd(',', ' ');

         return orderString;
      }

      private static string? GetValidPropName<T>(string? name)
      {
         if (string.IsNullOrEmpty(name))
         {
            return null;
         }

         var properties = typeof(T)
               .GetProperties(BindingFlags.Public | BindingFlags.Instance);

         //Searching property with passed name inside object
         var propName = properties
            .Where(p => p.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
            .Select(p => p.Name)
            .FirstOrDefault();

         return propName;
      }

      private static Expression<Func<T, bool>> BuildSearchExpression<T>(string propName,
         string term)
      {
         var parameter = Expression.Parameter(typeof(T), "entity");
         var filteringMember = Expression.PropertyOrField(parameter, propName);

         var termParam = Expression.Constant(term);

         MethodInfo toLowerMethod = typeof(string).GetMethod("ToLower",
            Type.EmptyTypes)!;
         MethodInfo containsMethod = typeof(string).GetMethod("Contains",
            [typeof(string)])!;

         var toLowerCall = Expression.Call(filteringMember, toLowerMethod);
         var methodCall = Expression.Call(toLowerCall, containsMethod, termParam);

         return Expression.Lambda<Func<T, bool>>(methodCall, parameter);
      }

      private static Expression<Func<T, bool>> BuildFilterExpression<T>(
         string propName, IQueryable<string> values)
      {
         var parameter = Expression.Parameter(typeof(T), "entity");
         var filteringMember = Expression.PropertyOrField(parameter, propName);

         var collection = Expression.Constant(values);

         MethodInfo methodInfo = typeof(Queryable).GetMethods()
            .Single(x => x.Name == "Contains" && x.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(string));

         var body = Expression.Call(null, methodInfo, collection, filteringMember);

         return Expression.Lambda<Func<T, bool>>(body, parameter);
      }

      #endregion

   }
}