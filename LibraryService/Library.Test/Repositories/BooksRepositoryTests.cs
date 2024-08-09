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
    public class BooksRepositoryTests : BaseRepositoryTest
    {
        [Fact]
        public async Task GetSingle_ShouldReturnValidBook()
        {
            //Arrange
            using var scope = provider.CreateScope();

            var targetId = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319");
            var repoManager = provider.GetRequiredService<IRepositoryManager>();

            //Act
            var book = await repoManager
                .Books
                .GetSingle(b => b.Id.Equals(targetId));

            //Assert
            Assert.NotNull(book);
            Assert.Equal(targetId, book.Id);
            Assert.Equal("War and Peace", book.Title);
        }

        [Fact]
        public async Task GetSingle_ShouldReturnNullIfNoMatching()
        {
            //Arrange
            using var scope = provider.CreateScope();

            var repoManager = provider.GetRequiredService<IRepositoryManager>();

            //Act
            var book = await repoManager
                .Books
                .GetSingle(b => b.Id.Equals(Guid.Empty));

            //Assert
            Assert.Null(book);
        }

        [Fact]
        public async Task GetRange_ShouldReturnBooksList()
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
            var books = await repoManager
                .Books
                .GetRange(parameters);

            //Assert
            Assert.NotNull(books);

            if (context.Books.Count() > parameters.PageSize)
            {
                Assert.Equal(parameters.PageSize, books.Count());
            }
            else
            {
                Assert.Equal(context.Books.Count(), books.Count());
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
            var books = await repoManager
                .Books
                .GetRange(parameters, b => b.Id.Equals(Guid.Empty));

            //Assert
            Assert.Empty(books);
        }


        [Fact]
        public async Task Add_ShouldCreateBook()
        {
            //Arrange
            using var scope = provider.CreateScope();

            var context = provider.GetRequiredService<RepositoryContext>();
            var repoManager = provider.GetRequiredService<IRepositoryManager>();

            var testAuthor = context.Authors.AsNoTracking().First();

            var bookForCreation = new Book
            {
                Title = "Test book",
                AuthorName = testAuthor.FirstName,
                AuthorId = testAuthor.Id,
                ISBN = "ISBN 13: 9781566190275",
                Genre = "Test",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit."
            };

            //Act
            repoManager
                .Books
                .Create(bookForCreation, default);
            await repoManager.SaveChangesAsync();
            context.ChangeTracker.Clear();

            //Assert
            Assert.NotEqual(Guid.Empty, bookForCreation.Id);

            var createdBook = context.Books.FirstOrDefault(b => b.Id.Equals(bookForCreation.Id));

            Assert.NotNull(createdBook);
            Assert.Equal(bookForCreation.Title, createdBook.Title);
        }

        [Fact]
        public async Task Update_ShouldUpdateBook()
        {
            //Arrange
            using var scope = provider.CreateScope();

            var context = provider.GetRequiredService<RepositoryContext>();
            var repoManager = provider.GetRequiredService<IRepositoryManager>();

            var bookForUpdate =
                context
                    .Books
                    .AsNoTracking()
                    .Single(b => b.Id.Equals(new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319")));
            bookForUpdate.Title = "Updated title";

            //Act
            repoManager
                .Books
                .Update(bookForUpdate, default);
            await repoManager.SaveChangesAsync();
            context.ChangeTracker.Clear();

            //Assert
            var updatedBook = context.Books.FirstOrDefault(b => b.Id.Equals(bookForUpdate.Id));

            Assert.NotNull(updatedBook);
            Assert.Equal("Updated title", updatedBook.Title);
        }

        [Fact]
        public async Task Delete_ShouldDeleteBook()
        {
            //Arrange
            using var scope = provider.CreateScope();

            var context = provider.GetRequiredService<RepositoryContext>();
            var repoManager = provider.GetRequiredService<IRepositoryManager>();

            var bookId = new Guid("14ca202e-dfb4-4d97-b7ef-76cf510bf319");
            var bookForDeletion =
                context
                    .Books
                    .AsNoTracking()
                    .Single(b => b.Id.Equals(bookId));

            //Act
            repoManager
                .Books
                .Delete(bookForDeletion, default);
            await repoManager.SaveChangesAsync();
            context.ChangeTracker.Clear();

            //Assert
            var deletedBook = context.Books.FirstOrDefault(b => b.Id.Equals(bookId));

            Assert.Null(deletedBook);
        }
    }
}