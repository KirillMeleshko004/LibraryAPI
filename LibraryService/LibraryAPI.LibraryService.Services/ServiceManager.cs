using AutoMapper;
using LibraryAPI.LibraryService.Domain.Interfaces.Loggers;
using LibraryAPI.LibraryService.Domain.Interfaces.Repos;
using LibraryAPI.LibraryService.Domain.Interfaces.Services;
using LibraryAPI.LibraryService.Services.Books;

namespace LibraryAPI.LibraryService.Services
{
    public class ServiceManager : IServiceManager
    {
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