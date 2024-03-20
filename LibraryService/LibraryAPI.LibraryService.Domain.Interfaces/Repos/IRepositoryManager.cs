namespace LibraryAPI.LibraryService.Domain.Interfaces.Repos
{
    public interface IRepositoryManager
    {
        IBookRepository Books { get; set; }

        Task SaveChangesAsync();
    }
}