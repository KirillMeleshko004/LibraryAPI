using System.Linq.Dynamic.Core;
using System.Text.Json;
using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Library.UseCases.Common.Interfaces;
using Library.UseCases.Common.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Test.Repositories
{
    public class AuthorsRepositoryTests : BaseRepositoryTest
    {
        [Fact]
        public async Task GetSingle_ShouldReturnValidAuthork()
        {
            //Arrange
            using var scope = provider.CreateScope();

            var targetId = new Guid("4dc4b580-7fb5-4c2a-938a-7e464116c7dd");
            var repoManager = provider.GetRequiredService<IRepositoryManager>();

            //Act
            var author = await repoManager
                .Authors
                .GetSingle(b => b.Id.Equals(targetId));

            //Assert
            Assert.NotNull(author);
            Assert.Equal(targetId, author.Id);
            Assert.Equal("Lev", author.FirstName);
        }

        [Fact]
        public async Task GetSingle_ShouldReturnNullIfNoMatching()
        {
            //Arrange
            using var scope = provider.CreateScope();

            var repoManager = provider.GetRequiredService<IRepositoryManager>();

            //Act
            var author = await repoManager
                .Authors
                .GetSingle(b => b.Id.Equals(Guid.Empty));

            //Assert
            Assert.Null(author);
        }

        [Fact]
        public async Task GetRange_ShouldReturnAuthorsList()
        {
            //Arrange
            using var scope = provider.CreateScope();

            var context = provider.GetRequiredService<RepositoryContext>();
            var repoManager = provider.GetRequiredService<IRepositoryManager>();

            var parameters = new RequestParameters()
            {
                PageNumber = 0,
                PageSize = 10
            };

            //Act
            var authors = await repoManager
                .Authors
                .GetRange(parameters);

            //Assert
            Assert.NotNull(authors);

            if (context.Authors.Count() > parameters.PageSize)
            {
                Assert.Equal(parameters.PageSize, authors.Count());
            }
            else
            {
                Assert.Equal(context.Authors.Count(), authors.Count());
            }
        }

        [Fact]
        public async Task GetRange_ShouldReturnEmptyListIfNoMatching()
        {
            //Arrange
            using var scope = provider.CreateScope();

            var context = provider.GetRequiredService<RepositoryContext>();
            var repoManager = provider.GetRequiredService<IRepositoryManager>();

            var parameters = new RequestParameters()
            {
                PageNumber = 0,
                PageSize = 10
            };

            //Act
            var authors = await repoManager
                .Authors
                .GetRange(parameters, b => b.Id.Equals(Guid.Empty));

            //Assert
            Assert.Empty(authors);
        }


        [Fact]
        public async Task Add_ShouldCreateAuthor()
        {
            //Arrange
            using var scope = provider.CreateScope();

            var context = provider.GetRequiredService<RepositoryContext>();
            var repoManager = provider.GetRequiredService<IRepositoryManager>();

            var testAuthor = context.Authors.AsNoTracking().First();

            var authorForCreation = new Author
            {
                FirstName = "Test",
                LastName = "Test",
                DateOfBirth = new DateOnly(1000, 1, 1),
                Country = "Test",
            };

            //Act
            repoManager
                .Authors
                .Create(authorForCreation, default);
            await repoManager.SaveChangesAsync();
            context.ChangeTracker.Clear();

            //Assert
            Assert.NotEqual(Guid.Empty, authorForCreation.Id);

            var createdAuthor = context.Authors
                .FirstOrDefault(b => b.Id.Equals(authorForCreation.Id));

            Assert.NotNull(createdAuthor);
            Assert.Equal(authorForCreation.FirstName, createdAuthor.FirstName);
        }

        [Fact]
        public async Task Update_ShouldUpdateAuthor()
        {
            //Arrange
            using var scope = provider.CreateScope();

            var context = provider.GetRequiredService<RepositoryContext>();
            var repoManager = provider.GetRequiredService<IRepositoryManager>();

            var authorForUpdate =
                context
                    .Authors
                    .AsNoTracking()
                    .Single(b => b.Id.Equals(new Guid("4dc4b580-7fb5-4c2a-938a-7e464116c7dd")));
            authorForUpdate.FirstName = "Updated name";

            //Act
            repoManager
                .Authors
                .Update(authorForUpdate, default);
            await repoManager.SaveChangesAsync();
            context.ChangeTracker.Clear();

            //Assert
            var updatedAuthor = context.Authors
                .FirstOrDefault(b => b.Id.Equals(authorForUpdate.Id));

            Assert.NotNull(updatedAuthor);
            Assert.Equal("Updated name", updatedAuthor.FirstName);
        }

        [Fact]
        public async Task Delete_ShouldDeleteAuthor()
        {
            //Arrange
            using var scope = provider.CreateScope();

            var context = provider.GetRequiredService<RepositoryContext>();
            var repoManager = provider.GetRequiredService<IRepositoryManager>();

            var authorId = new Guid("4dc4b580-7fb5-4c2a-938a-7e464116c7dd");
            var authorForDeletion =
                context
                    .Authors
                    .AsNoTracking()
                    .Single(b => b.Id.Equals(authorId));

            //Act
            repoManager
                .Authors
                .Delete(authorForDeletion, default);
            await repoManager.SaveChangesAsync();
            context.ChangeTracker.Clear();

            //Assert
            var deletedAuthor = context.Authors.FirstOrDefault(b => b.Id.Equals(authorId));

            Assert.Null(deletedAuthor);
        }
    }
}