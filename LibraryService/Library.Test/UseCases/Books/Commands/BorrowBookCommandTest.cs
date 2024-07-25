using System.Linq.Expressions;
using Library.Domain.Entities;
using Library.UseCases.Books.Commands;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Library.Test.UseCases.Books.Commands
{
    public class BorrowBookCommandTest
    {
        private readonly Mock<IRepositoryManager> _repoMock;
        private readonly Mock<ILogger<BorrowBookHandler>> _loggerMock;

        public BorrowBookCommandTest()
        {
            _repoMock = new();
            _loggerMock = new();
        }

        [Fact]
        public async Task Handle_Should_CallUpdateAndSaveMethods_IfBookIsAvailable()
        {
            //Arrange
            var command = new BorrowBookCommand("test@gmail.com", Guid.Empty);
            var handler = new BorrowBookHandler(_repoMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetSingle(
                    It.IsAny<Expression<Func<Book, bool>>>(),
                    It.IsAny<Expression<Func<Book, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Book() { IsAvailable = true });

            _repoMock.Setup(
                x => x.Readers.GetSingle(
                    It.IsAny<Expression<Func<Reader, bool>>>(),
                    It.IsAny<Expression<Func<Reader, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Reader());

            //Act
            await handler.Handle(command, default);

            //Assert
            _repoMock.Verify(
                x => x.Books.Update(It.IsAny<Book>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
            _repoMock.Verify(
                x => x.SaveChangesAsync(),
                Times.AtLeastOnce
            );
        }

        [Fact]
        public async Task Handle_Should_CallAddReaderMethod_IfReaderDontExist()
        {
            //Arrange
            var command = new BorrowBookCommand("test@gmail.com", Guid.Empty);
            var handler = new BorrowBookHandler(_repoMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetSingle(
                    It.IsAny<Expression<Func<Book, bool>>>(),
                    It.IsAny<Expression<Func<Book, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Book() { IsAvailable = true });

            _repoMock.SetupSequence(
                x => x.Readers.GetSingle(
                    It.IsAny<Expression<Func<Reader, bool>>>(),
                    It.IsAny<Expression<Func<Reader, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as Reader)
                .ReturnsAsync(new Reader());

            //Act
            await handler.Handle(command, default);

            //Assert
            _repoMock.Verify(
                x => x.Readers.Create(It.IsAny<Reader>(), It.IsAny<CancellationToken>()),
                Times.Once
            );
            _repoMock.Verify(
                x => x.SaveChangesAsync(),
                Times.AtLeastOnce
            );
        }

        [Fact]
        public async Task Handle_Should_UpdateBookState_IfBookIsAvailable()
        {
            //Arrange
            var command = new BorrowBookCommand("test@gmail.com", Guid.Empty);
            var handler = new BorrowBookHandler(_repoMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetSingle(
                    It.IsAny<Expression<Func<Book, bool>>>(),
                    It.IsAny<Expression<Func<Book, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Book() { IsAvailable = true });

            _repoMock.Setup(
                x => x.Readers.GetSingle(
                    It.IsAny<Expression<Func<Reader, bool>>>(),
                    It.IsAny<Expression<Func<Reader, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Reader() { Id = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319") });

            _repoMock.Setup(
                x => x.Books.Update(
                    It.IsAny<Book>(),
                    It.IsAny<CancellationToken>()
                )
            );

            //Act
            await handler.Handle(command, default);

            //Assert
            _repoMock.Verify(
                x => x.Books.Update(
                    It.Is<Book>(b => b.IsAvailable == false &&
                        b.CurrentReaderId.Equals(new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319")) &&
                        b.BorrowTime != null && b.ReturnTime != null),
                    It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_IfBookNotExist()
        {
            //Arrange
            var command = new BorrowBookCommand("test@gmail.com", Guid.Empty);
            var handler = new BorrowBookHandler(_repoMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetSingle(
                    It.IsAny<Expression<Func<Book, bool>>>(),
                    It.IsAny<Expression<Func<Book, object>>>(),
                    It.IsAny<CancellationToken>()))
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
        public async Task Handle_Should_ThrowUnavailableException_IfBookIsNotAvailable()
        {
            //Arrange
            var command = new BorrowBookCommand("test@gmail.com", Guid.Empty);
            var handler = new BorrowBookHandler(_repoMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Books.GetSingle(
                    It.IsAny<Expression<Func<Book, bool>>>(),
                    It.IsAny<Expression<Func<Book, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Book { IsAvailable = false });

            //Act
            try
            {
                await handler.Handle(command, default);

                //Assert
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsType<UnavailableException>(ex);
                Assert.Equal($"Book with id: {Guid.Empty} is unavailable now.",
                    ex.Message);
            }
        }

    }
}