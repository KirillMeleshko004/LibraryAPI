using System.Linq.Expressions;
using AutoMapper;
using Library.Domain.Entities;
using Library.UseCases.Books.Commands;
using Library.UseCases.Books.DTOs;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Library.Test.UseCases.Books.Commands
{
    public class CreateBookCommandTest
    {
        private readonly Mock<IRepositoryManager> _repoMock;
        private readonly Mock<ILogger<CreateBookHandler>> _loggerMock;
        private readonly Mock<IImageService> _imageMock;
        private readonly Mock<IMapper> _mapperMock;

        public CreateBookCommandTest()
        {
            _repoMock = new();
            _loggerMock = new();
            _imageMock = new();
            _mapperMock = new();
        }

        [Fact]
        public async Task Handle_Should_CallAddAndSaveMethods_IfAuthorExist()
        {
            //Arrange
            var command = new CreateBookCommand(new BookForCreationDto());
            var handler = new CreateBookHandler(_repoMock.Object, _mapperMock.Object,
                _imageMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Authors.GetSingle(
                    It.IsAny<Expression<Func<Author, bool>>>(),
                    It.IsAny<Expression<Func<Author, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Author());

            _repoMock.Setup(
                x => x.Books)
                .Returns(new Mock<IBookRepository>().Object);

            _mapperMock.Setup(
                x => x.Map<Book>(It.IsAny<object>()))
                .Returns(new Book());
            _mapperMock.Setup(
                x => x.Map<BookDto>(It.IsAny<object>()))
                .Returns(new BookDto());

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            _repoMock.Verify(
                x => x.Books.Create(It.IsAny<Book>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
            _repoMock.Verify(
                x => x.SaveChangesAsync(),
                Times.AtLeastOnce
            );
        }

        [Fact]
        public async Task Handle_Should_CallSaveImageMethod_IfImageRequested()
        {
            //Arrange
            var command = new CreateBookCommand(new BookForCreationDto()
            { ImageName = "test.png", Image = new MemoryStream() });

            var handler = new CreateBookHandler(_repoMock.Object, _mapperMock.Object,
                _imageMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Authors.GetSingle(
                    It.IsAny<Expression<Func<Author, bool>>>(),
                    It.IsAny<Expression<Func<Author, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Author());

            _repoMock.Setup(
                x => x.Books)
                .Returns(new Mock<IBookRepository>().Object);

            _mapperMock.Setup(
                x => x.Map<Book>(It.IsAny<object>()))
                .Returns(new Book());
            _mapperMock.Setup(
                x => x.Map<BookDto>(It.IsAny<object>()))
                .Returns(new BookDto());

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            _imageMock.Verify(
                x => x.SaveImageAsync(It.IsAny<Stream>(), It.IsAny<string>()),
                Times.Once
            );
        }

        delegate void MockAddBookAsyncCallback(Book book, CancellationToken ct);
        delegate BookDto MockMapBookDtoReturns(object src);

        [Fact]
        public async Task Handle_Should_ReturnCreatedBook_IfAuthorExist()
        {
            //Arrange
            var command = new CreateBookCommand(new BookForCreationDto());
            var handler = new CreateBookHandler(_repoMock.Object, _mapperMock.Object,
                _imageMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Authors.GetSingle(
                    It.IsAny<Expression<Func<Author, bool>>>(),
                    It.IsAny<Expression<Func<Author, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Author());

            _repoMock.Setup(
                x => x.Books.GetSingle(
                    It.IsAny<Expression<Func<Book, bool>>>(),
                    It.IsAny<Expression<Func<Book, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Book() { Id = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319") });

            _repoMock.Setup(
                x => x.Books.Create(It.IsAny<Book>(), It.IsAny<CancellationToken>()))
                .Callback(
                    new MockAddBookAsyncCallback((Book book, CancellationToken ct) =>
                    {
                        book.Id = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319");
                    })
                );

            _mapperMock.Setup(
                x => x.Map<Book>(It.IsAny<object>()))
                .Returns(new Book());

            _mapperMock.Setup(
                x => x.Map<BookDto>(It.IsAny<object>()))
                .Returns(new MockMapBookDtoReturns((b =>
                {
                    return new BookDto() { Id = (b as Book)!.Id };
                }))
            );

            //Act
            var result = await handler.Handle(command, default);

            //Assert

            Assert.Equal(new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319"), result.Id);
        }

        [Fact]
        public async Task Handle_Should_ThrowUnprocessableEntityException_IfAuthorDoesntExist()
        {
            //Arrange
            var command = new CreateBookCommand(new BookForCreationDto());
            var handler = new CreateBookHandler(_repoMock.Object, _mapperMock.Object,
                _imageMock.Object, _loggerMock.Object);

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
                Assert.IsType<UnprocessableEntityException>(ex);
                Assert.Equal($"Author with id: {Guid.Empty} was not found during book creation.",
                    ex.Message);
            }
        }

    }
}