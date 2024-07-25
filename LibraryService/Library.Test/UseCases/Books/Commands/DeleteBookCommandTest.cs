using System.Linq.Expressions;
using Library.Domain.Entities;
using Library.UseCases.Books.Commands;
using Library.UseCases.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;


namespace Library.Test.UseCases.Books.Commands
{
    public class DeleteBookCommandTest
    {
        private readonly Mock<IRepositoryManager> _repoMock;
        private readonly Mock<ILogger<DeleteBookHandler>> _loggerMock;
        private readonly Mock<IImageService> _imageMock;

        public DeleteBookCommandTest()
        {
            _repoMock = new();
            _loggerMock = new();
            _imageMock = new();
        }

        [Theory]
        [InlineData("14ca202e-dfb4-4d97-b7ef-76cf510bf319")]
        public async Task Handle_Should_CallDeleteAndSaveMethods_IfBookExist(Guid bookId)
        {
            //Arrange
            var command = new DeleteBookCommand(bookId);
            var handler = new DeleteBookHandler(_repoMock.Object, _imageMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetSingle(
                    It.IsAny<Expression<Func<Book, bool>>>(),
                    It.IsAny<Expression<Func<Book, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Book() { Id = bookId });

            //Act
            await handler.Handle(command, default);

            //Assert
            _repoMock.Verify(
                x => x.Books.Delete(It.Is<Book>(b => b.Id.Equals(bookId)),
                    It.IsAny<CancellationToken>()
            ));
            _repoMock.Verify(
                x => x.SaveChangesAsync(),
                Times.AtLeastOnce
            );
        }

        [Theory]
        [InlineData("14ca202e-dfb4-4d97-b7ef-76cf510bf319")]
        public async Task Handle_ShouldNot_CallDeleteMethod_IfBookNotExist(Guid bookId)
        {
            //Arrange
            var command = new DeleteBookCommand(bookId);
            var handler = new DeleteBookHandler(_repoMock.Object, _imageMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetSingle(
                    It.IsAny<Expression<Func<Book, bool>>>(),
                    It.IsAny<Expression<Func<Book, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as Book);

            //Act
            await handler.Handle(command, default);

            //Assert
            _repoMock.Verify(
                x => x.Books.Delete(It.IsAny<Book>(), It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

        [Theory]
        [InlineData("14ca202e-dfb4-4d97-b7ef-76cf510bf319")]
        public async Task Handle_Should_CallDeleteImageMethod_IfBookHaveImage(Guid bookId)
        {
            //Arrange
            var command = new DeleteBookCommand(bookId);
            var handler = new DeleteBookHandler(_repoMock.Object, _imageMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetSingle(
                    It.IsAny<Expression<Func<Book, bool>>>(),
                    It.IsAny<Expression<Func<Book, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Book() { Id = bookId, ImagePath = "testpath.png" });

            //Act
            await handler.Handle(command, default);

            //Assert
            _imageMock.Verify(
                x => x.DeleteImageAsync(It.IsAny<string>())
            );
        }

    }
}