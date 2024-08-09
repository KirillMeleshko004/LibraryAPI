using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Library.Infrastructure.Data
{
    public class ApplicationDesignTimeDbContextFactory :
        IDesignTimeDbContextFactory<RepositoryContext>
    {
        /// <summary>
        /// args order: Server, Database, User Id, Password
        /// </summary>
        public RepositoryContext CreateDbContext(string[] args)
        {
            //Db user and password should be provided by console
            //e.g.: dotnet ef database update -- librarydb testdb username password
            if (args.Length != 4)
            {
                throw new ArgumentException("Not all arguments provided.");
            }

            var server = args[0];
            var db = args[1];
            var user = args[2];
            var password = args[3];

            var optionsBuilder = new DbContextOptionsBuilder<RepositoryContext>();

            optionsBuilder.UseSqlServer($"Server={server};Database={db};User Id={user};Password={password};Trust Server Certificate=True;",
                x =>
                {
                    x.MigrationsAssembly(typeof(RepositoryContext).Assembly.FullName);
                });

            return new RepositoryContext(optionsBuilder.Options);
        }
    }
}