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
    public class UpdateBookCommandTest
    {
        private readonly Mock<IRepositoryManager> _repoMock;
        private readonly Mock<ILogger<UpdateBookHandler>> _loggerMock;
        private readonly Mock<IImageService> _imageMock;
        private readonly Mock<IMapper> _mapperMock;

        public UpdateBookCommandTest()
        {
            _repoMock = new();
            _loggerMock = new();
            _imageMock = new();
            _mapperMock = new();
        }

        [Fact]
        public async Task Handle_Should_CallUpdateAndSaveMethods_IfBookIsValid()
        {
            //Arrange
            var command = new UpdateBookCommand(new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319"),
                new BookForUpdateDto());
            var handler = new UpdateBookHandler(_repoMock.Object, _imageMock.Object,
                _mapperMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetBookByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Book, object>>>()))
                .ReturnsAsync(new Book());

            _repoMock.Setup(
                x => x.Authors.GetAuthorByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Author());

            _mapperMock.Setup(
                x => x.Map(It.IsAny<BookForUpdateDto>(),
                    It.IsAny<Book>()));

            _mapperMock.Setup(
                x => x.Map<BookDto>(It.IsAny<object>()))
                .Returns(new BookDto());

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            _repoMock.Verify(
                x => x.Books.UpdateBookAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
            _repoMock.Verify(
                x => x.SaveChangesAsync(),
                Times.AtLeastOnce
            );
        }

        [Fact]
        public async Task Handle_Should_CallSaveAndDeleteImageMethods_IfBookChanged()
        {
            //Arrange
            var command = new UpdateBookCommand(new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319"),
                new BookForUpdateDto()
                {
                    ImageName = "test.png",
                    Image = new MemoryStream()
                });

            var handler = new UpdateBookHandler(_repoMock.Object, _imageMock.Object,
                _mapperMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetBookByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Book, object>>>()))
                .ReturnsAsync(new Book() { ImagePath = "old.jpeg" });

            _repoMock.Setup(
                x => x.Authors.GetAuthorByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Author());

            _mapperMock.Setup(
                x => x.Map(It.IsAny<BookForUpdateDto>(),
                    It.IsAny<Book>()));

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
            _imageMock.Verify(
                x => x.DeleteImageAsync(It.IsAny<string>()),
                Times.Once
            );
        }

        delegate BookDto MockMapBookDtoReturnsReturns(object src);
        [Fact]
        public async Task Handle_Should_ReturnUpdateedBook_IfBookIsValid()
        {
            //Arrange
            var bookId = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319");

            var command = new UpdateBookCommand(bookId, new BookForUpdateDto());
            var handler = new UpdateBookHandler(_repoMock.Object, _imageMock.Object,
                _mapperMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetBookByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Book, object>>>()))
                .ReturnsAsync(new Book() { Id = bookId });

            _repoMock.Setup(
                x => x.Authors.GetAuthorByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Author());

            _mapperMock.Setup(
                x => x.Map(It.IsAny<BookForUpdateDto>(), It.IsAny<Book>()));

            _mapperMock.Setup(
                x => x.Map<BookDto>(It.IsAny<object>()))
                .Returns(new MockMapBookDtoReturnsReturns(b =>
                {
                    return new BookDto() { Id = (b as Book)!.Id };
                })
            );

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            Assert.Equal(bookId, result.Id);
        }

        [Fact]
        public async Task Handle_Should_ThrowUnprocessableEntityException_IfAuthorDoesntExist()
        {
            //Arrange
            var command = new UpdateBookCommand(Guid.Empty, new BookForUpdateDto());

            var handler = new UpdateBookHandler(_repoMock.Object, _imageMock.Object,
                _mapperMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetBookByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Book, object>>>()))
                .ReturnsAsync(new Book()
                {
                    AuthorId = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319")
                });

            _repoMock.Setup(
                x => x.Authors.GetAuthorByIdAsync(
                    It.IsAny<Guid>(),
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
                Assert.Equal($"Incorret author id: {Guid.Empty}. Author was not found.",
                    ex.Message);
            }
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_IfBookNotExist()
        {
            //Arrange
            var command = new UpdateBookCommand(Guid.Empty, new BookForUpdateDto());

            var handler = new UpdateBookHandler(_repoMock.Object, _imageMock.Object,
                _mapperMock.Object, _loggerMock.Object);

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
                Assert.Equal($"Book with id: {Guid.Empty} was not found.",
                    ex.Message);
            }
        }

    }
}