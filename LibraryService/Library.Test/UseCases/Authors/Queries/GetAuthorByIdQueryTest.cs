using System.Linq.Expressions;
using AutoMapper;
using Library.Domain.Entities;
using Library.UseCases.Authors.DTOs;
using Library.UseCases.Authors.Queries;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Library.Test.UseCases.Authors.Queries
{
    public class GetAuthorByIdQueryTest
    {
        private readonly Mock<IRepositoryManager> _repoMock;
        private readonly Mock<ILogger<GetAuthorByIdHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public GetAuthorByIdQueryTest()
        {
            _repoMock = new();
            _loggerMock = new();
            _mapperMock = new();
        }

        delegate AuthorDto MockMapAuthorDtoReturns(object src);
        [Fact]
        public async Task Handle_Should_ReturnAuthor_IfAuthorExist()
        {
            //Arrange
            var authorId = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319");

            var command = new GetAuthorByIdQuery(authorId);
            var handler = new GetAuthorByIdHandler(_repoMock.Object, _mapperMock.Object,
                _loggerMock.Object);

            _repoMock.Setup(
                x => x.Authors.GetSingle(
                    It.IsAny<Expression<Func<Author, bool>>>(),
                    It.IsAny<Expression<Func<Author, object>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Author() { Id = authorId });

            _mapperMock.Setup(
                x => x.Map<AuthorDto>(It.IsAny<object>()))
                .Returns(new MockMapAuthorDtoReturns(a =>
                {
                    return new AuthorDto() { Id = (a as Author)!.Id };
                }));

            //Act
            var result = await handler.Handle(command, default);

            //Assert
            Assert.Equal(authorId, result.Id);
        }

        [Fact]
        public async Task Handle_Should_ThrowNotFoundException_IfBookNotExist()
        {
            //Arrange
            var command = new GetAuthorByIdQuery(Guid.Empty);
            var handler = new GetAuthorByIdHandler(_repoMock.Object, _mapperMock.Object,
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