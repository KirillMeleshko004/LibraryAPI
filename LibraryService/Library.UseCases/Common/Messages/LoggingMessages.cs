namespace Library.UseCases.Common.Messages
{
   public static class LoggingMessages
   {
      public const string AuthorNotFoundLog = "Author with id: {Id} was not found.";
      public const string AuthorNotFoundBookCreationLog =
         "Author with id: {Id} was not found during book creation.";
      public const string IncorrectAuthorIdLog =
         "Incorret author id: {Id}. Author was not found.";
      public const string AuthorCreatedLog = "Author with id: {Id} was created.";
      public const string AuthorDeletedLog = "Author with id: {Id} was deleted.";

      public const string BookNotFoundLog = "Book with id: {Id} was not found.";
      public const string BookNotFoundISBNLog = "Book with ISBN: {ISBN} was not found.";
      public const string BookCreatedLog = "Book with id: {Id} was created.";
      public const string BookDeletedLog = "Book with id: {Id} was deleted.";

      public const string ReaderNotFoundLog = "Reader with email: {email} was not found.";
      public const string ReaderDontHaveBookLog = "Reader with email: {email} has not borrowed book with id: {id}.";
   }
}