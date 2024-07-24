using System.Linq.Expressions;
using AutoMapper;
using Library.Domain.Entities;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Books.Queries;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Library.Test.UseCases.Books.Queries
{
    public class GetBookByISBNQueryTest
    {
        private readonly Mock<IRepositoryManager> _repoMock;
        private readonly Mock<ILogger<GetBookByISBNHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public GetBookByISBNQueryTest()
        {
            _repoMock = new();
            _loggerMock = new();
            _mapperMock = new();
        }

        delegate BookDto MockMapBookDtoReturns(object src);
        [Fact]
        public async Task Handle_Should_ReturnBook_IfBookExist()
        {
            //Arrange
            var ISBN = "ISBN 13: 9781909156821";

            var command = new GetBookByISBNQuery(ISBN);
            var handler = new GetBookByISBNHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetBookByISBNAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Book, object>>>()))
                .ReturnsAsync(new Book() { ISBN = ISBN });

            _mapperMock.Setup(
                x => x.Map<BookDto>(It.IsAny<object>()))
                .Returns(new MockMapBookDtoReturns(b =>
                {
                    return new BookDto() { ISBN = (b as Book)!.ISBN };
                }));

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            Assert.Equal(ISBN, result.ISBN);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_IfBookNotExist()
        {
            //Arrange
            var ISBN = "ISBN 13: 9781909156821";

            var command = new GetBookByISBNQuery(ISBN);
            var handler = new GetBookByISBNHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetBookByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Book, object>>>()))
                .ReturnsAsync(null as Book);

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
                Assert.Equal($"Book with ISBN: {ISBN} was not found.", ex.Message);
            }
        }

    }
}