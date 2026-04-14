using Microsoft.EntityFrameworkCore;

namespace Post.Query.Infrastructure.DataAccess
{
    public class DatabaseContextFactory
    {
        private readonly Action<DbContextOptionsBuilder> _configureDBContext;
        public DatabaseContextFactory(Action<DbContextOptionsBuilder> configureDBContext)
        {
            _configureDBContext = configureDBContext;
        }

        public DatabaseContext CreateDbContext()
        {
            DbContextOptionsBuilder<DatabaseContext> optionsBuilder = new();
            _configureDBContext(optionsBuilder);
            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}