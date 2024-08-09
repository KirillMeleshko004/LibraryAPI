using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Library.UseCases.Common.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Test.Repositories
{
    public class BaseRepositoryTest : IDisposable
    {
        protected readonly ServiceProvider provider;
        private readonly SqliteConnection _connection;

        public BaseRepositoryTest()
        {
            var services = new ServiceCollection();

            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            services.AddDbContext<RepositoryContext>(options =>
                {
                    options.UseSqlite(_connection);
                });

            services.AddScoped<IRepositoryManager, RepositoryManager>();

            provider = services.BuildServiceProvider();

            using var scope = provider.CreateScope();
            var context = provider.GetRequiredService<RepositoryContext>();
            context.Database.EnsureCreated();
            context.ChangeTracker.Clear();
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}