using System.Linq.Expressions;
using AutoMapper;
using Library.Domain.Entities;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Books.Queries;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Common.RequestFeatures;
using Library.UseCases.Common.Utility;
using Library.UseCases.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Library.Test.UseCases.Books.Queries
{
    public class ListBooksForReaderQueryTest
    {
        private readonly Mock<IRepositoryManager> _repoMock;
        private readonly Mock<ILogger<ListBooksForReaderHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public ListBooksForReaderQueryTest()
        {
            _repoMock = new();
            _loggerMock = new();
            _mapperMock = new();
        }

        delegate IPagedEnumerable<BookDto> MockMapBookDtoReturns(object src);
        [Fact]
        public async Task Handle_Should_ReturnBooksList_IfReaderExist()
        {
            //Arrange
            var email = "test@test.com";

            var command = new ListBooksForReaderQuery(new RequestParameters(), email);
            var handler = new ListBooksForReaderHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Readers.GetSingle(
                    It.IsAny<Expression<Func<Reader, bool>>>(),
                    It.IsAny<Expression<Func<Reader, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Reader());

            _repoMock.Setup(
                x => x.Books.GetRange(
                    It.IsAny<RequestParameters>(),
                    It.IsAny<Expression<Func<Book, bool>>>(),
                    It.IsAny<Expression<Func<Book, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedList<Book>([new(), new(), new(), new()], new()));

            _mapperMock.Setup(
                x => x.Map<IPagedEnumerable<BookDto>>(It.IsAny<object>()))
                .Returns(new MockMapBookDtoReturns(bl =>
                {
                    var booksToReturn = new List<BookDto>();
                    foreach (var book in (bl as IEnumerable<Book>)!)
                    {
                        booksToReturn.Add(new BookDto());
                    }
                    return new PagedList<BookDto>(booksToReturn, new());
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
            var email = "test@test.com";

            var command = new ListBooksForReaderQuery(new RequestParameters(), email);
            var handler = new ListBooksForReaderHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Readers.GetSingle(
                    It.IsAny<Expression<Func<Reader, bool>>>(),
                    It.IsAny<Expression<Func<Reader, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Reader());

            _repoMock.Setup(
                x => x.Books.GetRange(
                    It.IsAny<RequestParameters>(),
                    It.IsAny<Expression<Func<Book, bool>>>(),
                    It.IsAny<Expression<Func<Book, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new PagedList<Book>([], new()));

            _mapperMock.Setup(
                x => x.Map<IPagedEnumerable<BookDto>>(It.IsAny<object>()))
                .Returns(new MockMapBookDtoReturns(bl =>
                {
                    var booksToReturn = new List<BookDto>();
                    foreach (var book in (bl as IEnumerable<Book>)!)
                    {
                        booksToReturn.Add(new BookDto());
                    }
                    return new PagedList<BookDto>(booksToReturn, new());
                }));

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_IfReaderNotFound()
        {
            //Arrange
            var email = "test@test.com";

            var command = new ListBooksForReaderQuery(new RequestParameters(), email);
            var handler = new ListBooksForReaderHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Readers.GetSingle(
                    It.IsAny<Expression<Func<Reader, bool>>>(),
                    It.IsAny<Expression<Func<Reader, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as Reader);

            //Act
            try
            {
                var result = await handler.Handle(command, default);

                //Assert
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsType<NotFoundException>(ex);
                Assert.Equal("Reader was not found.", ex.Message);
            }
        }

    }
}