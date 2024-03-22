namespace LibraryAPI.LibraryService.Domain.Interfaces.Services
{
    public interface IServiceManager
    {
        IBooksService Books { get; }
    }
}