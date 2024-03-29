using AutoMapper;
using LibraryAPI.LibraryService.Domain.Interfaces.Loggers;
using LibraryAPI.LibraryService.Domain.Interfaces.Repos;
using LibraryAPI.LibraryService.Domain.Interfaces.Services;
using LibraryAPI.LibraryService.Services.Books;

namespace LibraryAPI.LibraryService.Services
{
    /// <summary>
    /// Class represents single unit of work for services.
    /// ServiceManager instance created once for each request and ensures
    /// that all services use same instance of IRepositoryManager 
    /// </summary>
    public class ServiceManager : IServiceManager
    {
        //Make services Lazy to save resources if service not used during request
        private readonly Lazy<IBooksService> _books;

        public IBooksService Books 
        { 
            get
            {
                return _books.Value;
            }
        }

        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper,
            ILibraryLogger logger)
        {
            _books = new Lazy<IBooksService>(
                () => new BooksService(repositoryManager, logger, mapper)
            );
        }

    }
}