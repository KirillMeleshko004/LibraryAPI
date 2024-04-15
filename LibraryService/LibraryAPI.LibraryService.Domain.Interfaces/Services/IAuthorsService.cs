using LibraryAPI.LibraryService.Domain.Core.Results;
using LibraryAPI.LibraryService.Shared.DTOs;
using LibraryAPI.LibraryService.Shared.RequestFeatures;

namespace LibraryAPI.LibraryService.Domain.Interfaces.Services
{
    public interface IAuthorsService
    {
        Task<AuthorDto?> GetAuthorByIdAsync(Guid id);

        Task<IEnumerable<AuthorDto>> GetAuthorsAsync(AuthorParameters parameters);

        Task<ValueOpResult<AuthorDto>> CreateAuthorAsync(AuthorForCreationDto authorDto);
        Task<OpResult> UpdateAuthorAsync(Guid id, AuthorForUpdateDto authorDto);
        Task DeleteAuthorAsync(Guid id);
    }
}