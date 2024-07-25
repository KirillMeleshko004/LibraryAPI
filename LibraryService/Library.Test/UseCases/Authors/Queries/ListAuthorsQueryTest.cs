using System.Linq.Expressions;
using AutoMapper;
using Library.Domain.Entities;
using Library.UseCases.Authors.DTOs;
using Library.UseCases.Authors.Queries;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Books.Queries;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Common.RequestFeatures;
using Library.UseCases.Common.Utility;
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

        delegate IPagedEnumerable<AuthorDto> MockMapAuthorDtoReturns(object src);
        [Fact]
        public async Task Handle_Should_ReturnBooksList_IfBooksExist()
        {
            //Arrange
            var authorParameters = new RequestParameters();

            var command = new ListAuthorsQuery(authorParameters);
            var handler = new ListAuthorsHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Authors.GetRange(
                    It.IsAny<RequestParameters>(),
                    It.IsAny<Expression<Func<Author, bool>>>(),
                    It.IsAny<Expression<Func<Author, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedList<Author>([new(), new(), new(), new()], new()));

            _mapperMock.Setup(
                x => x.Map<IPagedEnumerable<AuthorDto>>(It.IsAny<object>()))
                .Returns(new MockMapAuthorDtoReturns(bl =>
                {
                    var authorsToReturn = new List<AuthorDto>();
                    foreach (var book in (bl as IEnumerable<Author>)!)
                    {
                        authorsToReturn.Add(new AuthorDto());
                    }
                    return new PagedList<AuthorDto>(authorsToReturn, new());
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
            var authorParameters = new RequestParameters();

            var command = new ListAuthorsQuery(authorParameters);
            var handler = new ListAuthorsHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Authors.GetRange(
                    It.IsAny<RequestParameters>(),
                    It.IsAny<Expression<Func<Author, bool>>>(),
                    It.IsAny<Expression<Func<Author, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedList<Author>([], new()));

            _mapperMock.Setup(
                x => x.Map<IPagedEnumerable<AuthorDto>>(It.IsAny<object>()))
                .Returns(new MockMapAuthorDtoReturns(al =>
                {
                    var authorsToReturn = new List<AuthorDto>();
                    foreach (var book in (al as IEnumerable<Author>)!)
                    {
                        authorsToReturn.Add(new AuthorDto());
                    }
                    return new PagedList<AuthorDto>(authorsToReturn, new());
                }));

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            Assert.Empty(result);
        }

    }
}