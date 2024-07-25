using AutoMapper;
using Library.UseCases.Authors.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using static Library.UseCases.Common.Messages.LoggingMessages;
using static Library.UseCases.Common.Messages.ResponseMessages;

namespace Library.UseCases.Authors.Commands
{
   public class UpdateAuthorHandler : IRequestHandler<UpdateAuthorCommand, AuthorDto>
   {
      private readonly IRepositoryManager _repo;
      private readonly IMapper _mapper;
      private readonly ILogger<UpdateAuthorHandler> _logger;

      public UpdateAuthorHandler(IRepositoryManager repo, IMapper mapper,
         ILogger<UpdateAuthorHandler> logger)
      {
         _repo = repo;
         _mapper = mapper;
         _logger = logger;
      }

      public async Task<AuthorDto> Handle(UpdateAuthorCommand request,
         CancellationToken cancellationToken)
      {
         var author = await _repo.Authors.GetSingle(a => a.Id.Equals(request.AuthorId),
            cancellationToken: cancellationToken);

         if (author == null)
         {
            _logger.LogWarning(AuthorNotFoundLog, request.AuthorId);

            throw new NotFoundException(string.Format(AuthorNotFound, request.AuthorId));
         }

         _mapper.Map(request.AuthorDto, author);
         _repo.Authors.Update(author, cancellationToken);
         await _repo.SaveChangesAsync();

         var authorToReturn = _mapper.Map<AuthorDto>(author);

         return authorToReturn;
      }
   }
}