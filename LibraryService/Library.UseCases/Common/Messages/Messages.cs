namespace Library.UseCases.Common.Messages
{
   public static class ResponseMessages
   {
      public const string AuthorNotFound = "Author with id: {0} was not found.";
      public const string AuthorCreated = "Author with id: {0} was created.";
      public const string AuthorDeleted = "Author with id: {0} was deleted.";

      public const string BookNotFound = "Book with id: {0} was not found.";
      public const string BookNotAvailable = "Book with id: {0} not available.";
      public const string BookNotFoundISBN = "Book with ISBN: {0} was not found.";
      public const string BookCreated = "Book with id: {0} was created.";
      public const string BookDeleted = "Book with id: {0} was deleted.";

      public const string ReaderNotFound = "Reader with email: {0} was not found.";
      public const string ReaderDontHaveBook = "Reader with email: {0} has not borrowed book with id: {1}.";
   }
}