using AutoMapper;
using Library.Domain.Entities;
using Library.UseCases.Authors.Commands;
using Library.UseCases.Authors.DTOs;
using Library.UseCases.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Library.Test.UseCases.Authors.Commands
{
    public class CreateAuthorCommandTest
    {
        private readonly Mock<IRepositoryManager> _repoMock;
        private readonly Mock<ILogger<CreateAuthorHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public CreateAuthorCommandTest()
        {
            _repoMock = new();
            _loggerMock = new();
            _mapperMock = new();
        }

        [Fact]
        public async Task Handle_Should_CallAddAndSaveMethods()
        {
            //Arrange
            var command = new CreateAuthorCommand(new AuthorForCreationDto());
            var handler = new CreateAuthorHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _mapperMock.Setup(
                x => x.Map<Author>(It.IsAny<object>()))
                .Returns(new Author());
            _repoMock.Setup(
                x => x.Authors.Create(
                    It.IsAny<Author>(),
                    It.IsAny<CancellationToken>())
                );

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            _repoMock.Verify(
                x => x.Authors.Create(It.IsAny<Author>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
            _repoMock.Verify(
                x => x.SaveChangesAsync(),
                Times.AtLeastOnce
            );
        }

        delegate void MockAddAuthorAsyncCallback(Author book, CancellationToken ct);
        delegate AuthorDto MockMapAuthorDtoReturns(object src);
        [Fact]
        public async Task Handle_Should_ReturnCreatedAuthor()
        {
            //Arrange
            var id = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319");

            var command = new CreateAuthorCommand(new AuthorForCreationDto());
            var handler = new CreateAuthorHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _mapperMock.Setup(
                x => x.Map<Author>(It.IsAny<object>()))
                .Returns(new Author());

            _repoMock.Setup(
                x => x.Authors.Create(
                    It.IsAny<Author>(),
                    It.IsAny<CancellationToken>())
                )
                .Callback(new MockAddAuthorAsyncCallback((a, ct) =>
                {
                    a.Id = id;
                }));

            _mapperMock.Setup(
                x => x.Map<AuthorDto>(It.IsAny<object>()))
                .Returns(new MockMapAuthorDtoReturns(a =>
                {
                    return new AuthorDto() { Id = (a as Author)!.Id };
                }));

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            Assert.Equal(id, result.Id);
        }
    }
}