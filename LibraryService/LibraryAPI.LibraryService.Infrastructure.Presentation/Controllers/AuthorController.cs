using LibraryAPI.LibraryService.Domain.Core.Results;
using LibraryAPI.LibraryService.Domain.Interfaces.Loggers;
using LibraryAPI.LibraryService.Domain.Interfaces.Services;
using LibraryAPI.LibraryService.Shared.DTOs;
using LibraryAPI.LibraryService.Shared.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.LibraryService.Infrastructure.Presentation.Controllers
{
   [ApiController]
   [Route("api/authors")]
   public class AuthorController : ControllerBase
   {
      private readonly IServiceManager _services;
      private readonly ILibraryLogger _logger;

      public AuthorController(IServiceManager serviceManager, ILibraryLogger logger)
      {
         _services = serviceManager;
         _logger = logger;
      }

      [HttpGet]
      [ProducesResponseType(StatusCodes.Status200OK)]
      public async Task<IActionResult> GetAuthors([FromQuery] AuthorParameters parameters)
      {
         var authors = await _services.Authors.GetAuthorsAsync(parameters);

         return Ok(authors);
      }

      [HttpGet("{id:guid}")]
      [ProducesResponseType(StatusCodes.Status200OK)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> GetAuthorById(Guid id)
      {
         var author = await _services.Authors.GetAuthorByIdAsync(id);

         if (author == null)
         {
            return NotFound($"Author with id: {id} not found.");
         }

         return Ok(author);
      }

      [HttpPost]
      [ProducesResponseType(StatusCodes.Status201Created)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      public async Task<IActionResult> CreateAuthor([FromBody] AuthorForCreationDto authorDto)
      {

         var res = await _services.Authors.CreateAuthorAsync(authorDto);

         if (res.Status == OpStatus.Fail)
         {
            return StatusCode((int)res.Error!.ErrorType,
               new { message = res.Error.Description });
         }

         return CreatedAtAction(nameof(GetAuthorById),
            new { id = res.Value!.Id },
            res.Value);

      }

      [HttpPut("{id:guid}")]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status400BadRequest)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      [ProducesResponseType(StatusCodes.Status404NotFound)]
      public async Task<IActionResult> UpdateAuthor(Guid id,
         [FromBody] AuthorForUpdateDto authorDto)
      {
         var res = await _services.Authors.UpdateAuthorAsync(id, authorDto);

         if (res.Status == OpStatus.Fail)
         {
            return StatusCode((int)res.Error!.ErrorType,
               new { message = res.Error.Description });
         }

         return NoContent();
      }

      [HttpDelete("{id:guid}")]
      [ProducesResponseType(StatusCodes.Status204NoContent)]
      [ProducesResponseType(StatusCodes.Status401Unauthorized)]
      public async Task<IActionResult> DeleteAuthor(Guid id)
      {
         await _services.Authors.DeleteAuthorAsync(id);

         return NoContent();
      }


   }
}