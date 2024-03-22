namespace LibraryAPI.LibraryService.Domain.Core.Exceptions
{
    public class BookNotFoundException : NotFoundException
    {

        public BookNotFoundException(Guid id)
            : base($"Book with id: {id} don't exist in database.")
        {
            
        }

        public BookNotFoundException(string ISBN)
            : base($"Book with ISBN: {ISBN} don't exist in database.")
        {
            
        }
        
    }
}