using AutoMapper;
using LibraryAPI.LibraryService.Domain.Core.Entities;
using LibraryAPI.LibraryService.Domain.Core.Results;
using LibraryAPI.LibraryService.Domain.Interfaces.Loggers;
using LibraryAPI.LibraryService.Domain.Interfaces.Repos;
using LibraryAPI.LibraryService.Domain.Interfaces.Services;
using LibraryAPI.LibraryService.Shared.DTOs;
using LibraryAPI.LibraryService.Shared.RequestFeatures;

namespace LibraryAPI.LibraryService.Services.Authors
{
   public class AuthorsService : IAuthorsService
   {

      private readonly IRepositoryManager _repo;
      private readonly ILibraryLogger _logger;
      private readonly IMapper _mapper;

      public AuthorsService(IRepositoryManager respository, ILibraryLogger logger,
         IMapper mapper)
      {
         _repo = respository;
         _logger = logger;
         _mapper = mapper;
      }

      public async Task<AuthorDto?> GetAuthorByIdAsync(Guid id)
      {
         var authorRes = await GetAuthorByIdAsync(id, trackChanges: false);

         if (authorRes.Status == OpStatus.Fail) return null;

         var authorDto = _mapper.Map<AuthorDto>(authorRes.Value);

         return authorDto;
      }

      public async Task<IEnumerable<AuthorDto>> GetAuthorsAsync(AuthorParameters parameters)
      {
         var authors = await _repo.Authors.GetAuthorsAsync(parameters, false);

         var authorToReturn = _mapper.Map<IEnumerable<AuthorDto>>(authors);

         return authorToReturn;
      }

      public async Task<ValueOpResult<AuthorDto>>
         CreateAuthorAsync(AuthorForCreationDto authorDto)
      {
         var author = _mapper.Map<Author>(authorDto);

         await _repo.Authors.AddAuthorAsync(author);
         await _repo.SaveChangesAsync();

         _logger.LogInfo($"Author with id: {author.Id} was created.");

         var authorToReturn = _mapper.Map<AuthorDto>(author);

         return OpResult.SuccessValueResult(authorToReturn);
      }

      public async Task<OpResult> UpdateAuthorAsync(Guid id, AuthorForUpdateDto authorDto)
      {
         var authorRes = await GetAuthorByIdAsync(id, trackChanges: true);

         if (authorRes.Status == OpStatus.Fail) return authorRes;

         //Since we track change of author entity, simple mapping dto into entity
         //override it fields
         _mapper.Map(authorDto, authorRes.Value);

         await _repo.SaveChangesAsync();

         return OpResult.SuccessResult();
      }

      public async Task DeleteAuthorAsync(Guid id)
      {
         var author = await _repo.Authors.GetAuthorByIdAsync(id, trackChanges: true);

         //If author not exist or already deleted
         if (author == null) return;

         await _repo.Authors.DeleteAuthorAsync(author);

         _logger.LogInfo($"Author with id: {author.Id} was deleted.");
      }


      #region Private methods

      private async Task<ValueOpResult<Author>> GetAuthorByIdAsync(Guid id, bool trackChanges)
      {
         var author = await _repo.Authors.GetAuthorByIdAsync(id, trackChanges);

         if (author == null)
         {
            var errMes = $"Author with id: {id} was not found.";
            _logger.LogWarn(errMes);
            return OpResult.FailValueResult<Author>(errMes, ErrorType.NotFound);
         }

         return OpResult.SuccessValueResult(author);
      }

      #endregion

   }
}