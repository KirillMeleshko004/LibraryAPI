using AutoMapper;
using Library.UseCases.Authors.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Authors.Queries
{
   public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDto>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger<GetAuthorByIdHandler> _logger;

      public GetAuthorByIdHandler(IRepositoryManager repo, IMapper mapper,
         ILogger<GetAuthorByIdHandler> logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<AuthorDto> Handle(GetAuthorByIdQuery request,
         CancellationToken cancellationToken)
      {
         var author = await _repo.Authors.GetSingle(a => a.Id.Equals(request.Id),
            cancellationToken: cancellationToken);

         if (author == null)
         {
            throw new NotFoundException(string.Format(AuthorNotFound, request.Id));
         }

         var authorToReturn = _mapper.Map<AuthorDto>(author);

         return authorToReturn;
      }
   }
}