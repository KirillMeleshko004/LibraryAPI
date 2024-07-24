using Library.Domain.Entities;
using Library.UseCases.Authors.Commands;
using Library.UseCases.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Library.Test.UseCases.Authors.Commands
{
    public class DeleteAuthorCommandTest
    {
        private readonly Mock<IRepositoryManager> _repoMock;
        private readonly Mock<ILogger<DeleteAuthorHandler>> _loggerMock;

        public DeleteAuthorCommandTest()
        {
            _repoMock = new();
            _loggerMock = new();
        }

        [Theory]
        [InlineData("14ca202e-dfb4-4d97-b7ef-76cf510bf319")]
        public async Task Handle_Should_CallDeleteAndSaveMethods_IfAuthorExist(Guid authorId)
        {
            //Arrange
            var command = new DeleteAuthorCommand(authorId);
            var handler = new DeleteAuthorHandler(_repoMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Authors.GetAuthorByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Author() { Id = authorId });

            //Act
            await handler.Handle(command, default);

            //Assert
            _repoMock.Verify(
                x => x.Authors.DeleteAuthorAsync(It.Is<Author>(a => a.Id.Equals(authorId)),
                    It.IsAny<CancellationToken>()
            ));
            _repoMock.Verify(
                x => x.SaveChangesAsync(),
                Times.AtLeastOnce
            );
        }

        [Theory]
        [InlineData("14ca202e-dfb4-4d97-b7ef-76cf510bf319")]
        public async Task Handle_ShouldNot_CallDeleteMethod_IfAuthorNotExist(Guid authorId)
        {
            //Arrange
            var command = new DeleteAuthorCommand(authorId);
            var handler = new DeleteAuthorHandler(_repoMock.Object, _loggerMock.Object);

            _repoMock.Setup(
                x => x.Authors.GetAuthorByIdAsync(
                    It.IsAny<Guid>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(null as Author);

            //Act
            await handler.Handle(command, default);

            //Assert
            _repoMock.Verify(
                x => x.Authors.DeleteAuthorAsync(It.IsAny<Author>(), It.IsAny<CancellationToken>()),
                Times.Never
            );
        }

    }
}