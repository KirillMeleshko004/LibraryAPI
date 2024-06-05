namespace Library.UseCases.Common.Messages
{

   public static class ResponseMessages
   {
      public static string AuthorsNotFound => "Authors with corresponding parameters were not found.";
      public static string AuthorNotFound => "Author with id: {0} was not found.";
      public static string AuthorNotFoundBookCreation =>
         "Author with id: {0} was not found during book creation.";
      public static string IncorrectAuthorId =>
         "Incorret author id: {0}. Author was not found.";
      public static string AuthorCreated => "Author with id: {0} was created.";
      public static string AuthorDeleted => "Author with id: {0} was deleted.";

      public static string BooksNotFound => "Books with corresponding parameters were not found.";
      public static string BookNotFound => "Book with id: {0} was not found.";
      public static string BookNotFoundISBN => "Book with ISBN: {0} was not found.";
      public static string BookCreated => "Book with id: {0} was created.";
      public static string BookDeleted => "Book with id: {0} was deleted.";

      public static string BookNotAvailable => "Book with id: {0} is unavailable now.";

      public static string ReaderNotFound => "Reader was not found.";
      public static string ReaderDontHaveBook => "Reader has not borrowed that book.";
   }
}