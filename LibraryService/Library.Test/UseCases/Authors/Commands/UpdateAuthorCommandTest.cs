using AutoMapper;
using Library.Domain.Entities;
using Library.UseCases.Authors.Commands;
using Library.UseCases.Authors.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Library.Test.UseCases.Authors.Commands
{
    public class UpdateAuthorCommandTest
    {
        private readonly Mock<IRepositoryManager> _repoMock;
        private readonly Mock<ILogger<UpdateAuthorHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public UpdateAuthorCommandTest()
        {
            _repoMock = new();
            _loggerMock = new();
            _mapperMock = new();
        }

        [Fact]
        public async Task Handle_Should_CallUpdateAndSaveMethods_IfAuthorIsValid()
        {
            //Arrange
            var authorId = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319");

            var command = new UpdateAuthorCommand(authorId, new AuthorForUpdateDto());
            var handler = new UpdateAuthorHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Authors.GetAuthorByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new Author() { Id = authorId });

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            _repoMock.Verify(
                x => x.Authors.UpdateAuthorAsync(It.Is<Author>(a => a.Id.Equals(authorId)),
                    It.IsAny<CancellationToken>()),
                Times.AtLeastOnce
            );
            _repoMock.Verify(
                x => x.SaveChangesAsync(),
                Times.AtLeastOnce
            );
        }

        delegate AuthorDto MockMapAuthorDtoReturns(object src);
        delegate void MockUpdateAuthorCallback(Author author, CancellationToken ct);
        [Fact]
        public async Task Handle_Should_ReturnUpdatedBook_IfAuthorIsValid()
        {
            //Arrange
            var authorId = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319");

            var command = new UpdateAuthorCommand(authorId, new AuthorForUpdateDto());
            var handler = new UpdateAuthorHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _mapperMock.Setup(
                x => x.Map<Author>(It.IsAny<object>()))
                .Returns(new Author());

            _repoMock.Setup(
                x => x.Authors.GetAuthorByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(new Author() { Id = authorId });


            _repoMock.Setup(
                x => x.Authors.UpdateAuthorAsync(
                    It.IsAny<Author>(),
                    It.IsAny<CancellationToken>()
                ))
            .Callback(new MockUpdateAuthorCallback((a, ct) =>
            {
                a.FirstName = "Updated name";
            }));

            _mapperMock.Setup(
                x => x.Map<AuthorDto>(It.IsAny<object>()))
                .Returns(new MockMapAuthorDtoReturns(a =>
                {
                    return new AuthorDto()
                    {
                        Id = (a as Author)!.Id,
                        FirstName = (a as Author)!.FirstName,
                    };
                }));

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            Assert.Equal(authorId, result.Id);
            Assert.Equal("Updated name", result.FirstName);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_IfAuthorNotFound()
        {
            //Arrange
            var command = new UpdateAuthorCommand(Guid.Empty, new AuthorForUpdateDto());
            var handler = new UpdateAuthorHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Authors.GetAuthorByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()
                )
            ).ReturnsAsync(null as Author);

            //Act
            try
            {
                await handler.Handle(command, default);

                //Assert
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsType<NotFoundException>(ex);
                Assert.Equal($"Author with id: {Guid.Empty} was not found.",
                    ex.Message);
            }
        }

    }
}