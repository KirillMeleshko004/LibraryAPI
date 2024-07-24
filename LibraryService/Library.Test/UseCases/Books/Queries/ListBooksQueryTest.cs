using System.Linq.Expressions;
using AutoMapper;
using Library.Domain.Entities;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Books.Queries;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Common.RequestFeatures;
using Microsoft.Extensions.Logging;
using Moq;

namespace Library.Test.UseCases.Books.Queries
{
    public class ListBooksQueryTest
    {
        private readonly Mock<IRepositoryManager> _repoMock;
        private readonly Mock<ILogger<ListBooksHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public ListBooksQueryTest()
        {
            _repoMock = new();
            _loggerMock = new();
            _mapperMock = new();
        }

        delegate IEnumerable<BookDto> MockMapBookDtoReturns(object src);
        [Fact]
        public async Task Handle_Should_ReturnBooksList_IfBooksExist()
        {
            //Arrange
            var bookParams = new BookParameters();

            var command = new ListBooksQuery(new BookParameters());
            var handler = new ListBooksHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetBooksAsync(
                    It.IsAny<BookParameters>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Book, object>>>()))
                .ReturnsAsync(new List<Book>()
                {
                    new(), new(), new(), new()
                });

            _mapperMock.Setup(
                x => x.Map<IEnumerable<BookDto>>(It.IsAny<object>()))
                .Returns(new MockMapBookDtoReturns(bl =>
                {
                    var booksToReturn = new List<BookDto>();
                    foreach (var book in (bl as IEnumerable<Book>)!)
                    {
                        booksToReturn.Add(new BookDto());
                    }
                    return booksToReturn;
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
            var bookParams = new BookParameters();

            var command = new ListBooksQuery(new BookParameters());
            var handler = new ListBooksHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetBooksAsync(
                    It.IsAny<BookParameters>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Book, object>>>()))
                .ReturnsAsync([]);

            _mapperMock.Setup(
                x => x.Map<IEnumerable<BookDto>>(It.IsAny<object>()))
                .Returns(new MockMapBookDtoReturns(bl =>
                {
                    var booksToReturn = new List<BookDto>();
                    foreach (var book in (bl as IEnumerable<Book>)!)
                    {
                        booksToReturn.Add(new BookDto());
                    }
                    return booksToReturn;
                }));

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            Assert.Empty(result);
        }

    }
}