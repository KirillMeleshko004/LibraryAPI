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
    public class ListBooksByAuthorQueryTest
    {
        private readonly Mock<IRepositoryManager> _repoMock;
        private readonly Mock<ILogger<ListBooksByAuthorHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public ListBooksByAuthorQueryTest()
        {
            _repoMock = new();
            _loggerMock = new();
            _mapperMock = new();
        }

        delegate IPagedEnumerable<BookDto> MockMapBookDtoReturns(object src);
        [Fact]
        public async Task Handle_Should_ReturnBooksList_IfBooksExist()
        {
            //Arrange
            var authorId = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319");
            var bookParams = new RequestParameters();

            var command = new ListBooksByAuthorQuery(bookParams, authorId);
            var handler = new ListBooksByAuthorHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Authors.GetSingle(
                    It.IsAny<Expression<Func<Author, bool>>>(),
                    It.IsAny<Expression<Func<Author, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Author());

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
            var authorId = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319");
            var bookParams = new RequestParameters();

            var command = new ListBooksByAuthorQuery(bookParams, authorId);
            var handler = new ListBooksByAuthorHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);


            _repoMock.Setup(
                x => x.Authors.GetSingle(
                    It.IsAny<Expression<Func<Author, bool>>>(),
                    It.IsAny<Expression<Func<Author, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Author());

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
        public async Task Handle_Should_ThrowNotFoundException_IfAuthorDoesntExist()
        {
            //Arrange
            var command = new ListBooksByAuthorQuery(new RequestParameters(), Guid.Empty);
            var handler = new ListBooksByAuthorHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Authors.GetSingle(
                    It.IsAny<Expression<Func<Author, bool>>>(),
                    It.IsAny<Expression<Func<Author, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as Author);

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
                Assert.Equal($"Author with id: {Guid.Empty} was not found.", ex.Message);
            }
        }

    }
}