using AutoMapper;
using Library.Domain.Entities;
using Library.UseCases.Authors.DTOs;
using Library.UseCases.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Library.UseCases.Authors.Commands
{
   public class CreateAuthorHandler : IRequestHandler<CreateAuthorCommand, AuthorDto>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger<CreateAuthorHandler> _logger;

      public CreateAuthorHandler(IRepositoryManager repo, IMapper mapper,
         ILogger<CreateAuthorHandler> logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<AuthorDto> Handle(CreateAuthorCommand request,
         CancellationToken cancellationToken)
      {
         var author = _mapper.Map<Author>(request.AuthorDto);

         _repo.Authors.Create(author, cancellationToken);
         await _repo.SaveChangesAsync();

         var authorToReturn = _mapper.Map<AuthorDto>(author);

         return authorToReturn;
      }
   }
}