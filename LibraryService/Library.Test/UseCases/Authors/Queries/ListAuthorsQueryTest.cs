using System.Linq.Expressions;
using AutoMapper;
using Library.Domain.Entities;
using Library.UseCases.Authors.DTOs;
using Library.UseCases.Authors.Queries;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Books.Queries;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Common.RequestFeatures;
using Microsoft.Extensions.Logging;
using Moq;

namespace Library.Test.UseCases.Books.Queries
{
    public class ListAuthorsQueryTest
    {
        private readonly Mock<IRepositoryManager> _repoMock;
        private readonly Mock<ILogger<ListAuthorsHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public ListAuthorsQueryTest()
        {
            _repoMock = new();
            _loggerMock = new();
            _mapperMock = new();
        }

        delegate IEnumerable<AuthorDto> MockMapAuthorDtoReturns(object src);
        [Fact]
        public async Task Handle_Should_ReturnBooksList_IfBooksExist()
        {
            //Arrange
            var authorParameters = new AuthorParameters();

            var command = new ListAuthorsQuery(authorParameters);
            var handler = new ListAuthorsHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Authors.GetAuthorsAsync(
                    It.IsAny<AuthorParameters>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Author>()
                {
                    new(), new(), new(), new()
                });

            _mapperMock.Setup(
                x => x.Map<IEnumerable<AuthorDto>>(It.IsAny<object>()))
                .Returns(new MockMapAuthorDtoReturns(bl =>
                {
                    var authorsToReturn = new List<AuthorDto>();
                    foreach (var book in (bl as IEnumerable<Author>)!)
                    {
                        authorsToReturn.Add(new AuthorDto());
                    }
                    return authorsToReturn;
                }));

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            Assert.Equal(4, result.Count());
        }

        [Fact]
        public async Task Handle_Should_ReturnEmptyList_IfNoBooksMatchParams()
        {
            //Arrange
            var authorParameters = new AuthorParameters();

            var command = new ListAuthorsQuery(authorParameters);
            var handler = new ListAuthorsHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Authors.GetAuthorsAsync(
                    It.IsAny<AuthorParameters>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            _mapperMock.Setup(
                x => x.Map<IEnumerable<AuthorDto>>(It.IsAny<object>()))
                .Returns(new MockMapAuthorDtoReturns(al =>
                {
                    var authorsToReturn = new List<AuthorDto>();
                    foreach (var book in (al as IEnumerable<Author>)!)
                    {
                        authorsToReturn.Add(new AuthorDto());
                    }
                    return authorsToReturn;
                }));

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            Assert.Empty(result);
        }

    }
}