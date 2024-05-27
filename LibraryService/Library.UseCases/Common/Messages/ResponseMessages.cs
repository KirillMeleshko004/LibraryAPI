namespace Library.UseCases.Common.Messages
{
   public static class ResponseMessages
   {
      public static string AuthorNotFound => "Author with id: {0} was not found.";
      public static string AuthorNotFoundBookCreation => 
         "Author with id: {Id} was not found during book creation.";
      public static string AuthorNotFoundBookUpdate => 
         "Author with id: {Id} was not found during book update.";
      public static string AuthorCreated => "Author with id: {0} was created.";
      public static string AuthorDeleted => "Author with id: {0} was deleted.";

      public static string BookNotFound => "Book with id: {0} was not found.";
      public static string BookNotFoundISBN => "Book with ISBN: {0} was not found.";
      public static string BookCreated => "Book with id: {0} was created.";
      public static string BookDeleted => "Book with id: {0} was deleted.";
   }
}