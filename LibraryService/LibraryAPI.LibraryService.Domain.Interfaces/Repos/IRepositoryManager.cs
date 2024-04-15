namespace LibraryAPI.LibraryService.Domain.Interfaces.Repos
{
    public interface IRepositoryManager
    {
        IBookRepository Books { get; }

        IAuthorRepository Authors { get; }

        Task SaveChangesAsync();
    }
}