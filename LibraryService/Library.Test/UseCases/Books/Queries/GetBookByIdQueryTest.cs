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
    public class GetBookByIdQueryTest
    {
        private readonly Mock<IRepositoryManager> _repoMock;
        private readonly Mock<ILogger<GetBookByIdHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public GetBookByIdQueryTest()
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
            var bookId = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319");

            var command = new GetBookByIdQuery(bookId);
            var handler = new GetBookByIdHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetSingle(
                    It.IsAny<Expression<Func<Book, bool>>>(),
                    It.IsAny<Expression<Func<Book, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Book() { Id = bookId });

            _mapperMock.Setup(
                x => x.Map<BookDto>(It.IsAny<object>()))
                .Returns(new MockMapBookDtoReturns(b =>
                {
                    return new BookDto() { Id = (b as Book)!.Id };
                }));

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            Assert.Equal(bookId, result.Id);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_IfBookNotExist()
        {
            //Arrange
            var command = new GetBookByIdQuery(Guid.Empty);
            var handler = new GetBookByIdHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetSingle(
                    It.IsAny<Expression<Func<Book, bool>>>(),
                    It.IsAny<Expression<Func<Book, object>>>(),
                    It.IsAny<CancellationToken>()))
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
                Assert.Equal($"Book with id: {Guid.Empty} was not found.", ex.Message);
            }
        }

    }
}