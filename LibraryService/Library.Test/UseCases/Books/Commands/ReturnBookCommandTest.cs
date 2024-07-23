using System.Linq.Expressions;
using Library.Domain.Entities;
using Library.UseCases.Books.Commands;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Library.Test.UseCases.Books.Commands
{
    public class ReturnBookCommandTest
    {
        private readonly Mock<IRepositoryManager> _repoMock;
        private readonly Mock<ILogger<ReturnBookHandler>> _loggerMock;

        public ReturnBookCommandTest()
        {
            _repoMock = new();
            _loggerMock = new();
        }

        [Fact]
        public async Task Handle_Should_CallUpdateAndSaveMethods_IfBookAndReaderValid()
        {
            //Arrange
            var command = new ReturnBookCommand("test@gmail.com", Guid.Empty);
            var handler = new ReturnBookHandler(_repoMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetBookByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Book, object>>>()))
                .ReturnsAsync(new Book() { CurrentReaderId = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319") });

            _repoMock.Setup(
                x => x.Readers.GetReaderByEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Reader() { Id = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319") });

            //Act
            await handler.Handle(command, default);

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
        public async Task Handle_Should_UpdateBookState_IfBookAndReaderValid()
        {
            //Arrange
            var command = new ReturnBookCommand("test@gmail.com", Guid.Empty);
            var handler = new ReturnBookHandler(_repoMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetBookByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Book, object>>>()))
                .ReturnsAsync(new Book() { CurrentReaderId = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319") });

            _repoMock.Setup(
                x => x.Readers.GetReaderByEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Reader() { Id = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319") });

            //Act
            await handler.Handle(command, default);

            //Assert
            _repoMock.Verify(
                x => x.Books.UpdateBookAsync(
                    It.Is<Book>(b => b.IsAvailable == true &&
                        b.CurrentReaderId.Equals(null) &&
                        b.BorrowTime == null && b.ReturnTime == null),
                    It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_IfBookNotExist()
        {
            //Arrange
            var command = new ReturnBookCommand("test@gmail.com", Guid.Empty);
            var handler = new ReturnBookHandler(_repoMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Readers.GetReaderByEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Reader());

            _repoMock.Setup(
                x => x.Books.GetBookByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Book, object>>>()))
                .ReturnsAsync(null as Book);

            //Act
            try
            {
                await handler.Handle(command, default);

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

        [Fact]
        public async Task Handle_Should_ThrowUnauthorizedException_IfReaderNotFound()
        {
            //Arrange
            var command = new ReturnBookCommand("test@gmail.com", Guid.Empty);
            var handler = new ReturnBookHandler(_repoMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetBookByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Book, object>>>()))
                .ReturnsAsync(new Book());

            _repoMock.SetupSequence(
                x => x.Readers.GetReaderByEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as Reader);

            //Act
            try
            {
                await handler.Handle(command, default);

                //Assert
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsType<UnauthorizedException>(ex);
                Assert.Equal("Invalid reader.",
                    ex.Message);
            }
        }

        [Fact]
        public async Task Handle_Should_ThrowForbidException_IfReaderInvalid()
        {
            //Arrange
            var command = new ReturnBookCommand("test@gmail.com", Guid.Empty);
            var handler = new ReturnBookHandler(_repoMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetBookByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Expression<Func<Book, object>>>()))
                .ReturnsAsync(new Book());

            _repoMock.SetupSequence(
                x => x.Readers.GetReaderByEmailAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Reader() { Id = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319") });

            //Act
            try
            {
                await handler.Handle(command, default);

                //Assert
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsType<ForbidException>(ex);
                Assert.Equal("Reader has not borrowed that book.", ex.Message);
            }
        }

    }
}