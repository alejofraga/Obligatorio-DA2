using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace SmartHome.DataLayer.Test;

internal static class DbContextBuilder
{
    private static readonly SqliteConnection Connection = new("DataSource=:memory:");
    public static TestDbContext BuildTestDbContext()
    {
        DbContextOptions<TestDbContext> options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(Connection)
            .Options;

        Connection.Open();

        var context = new TestDbContext(options);

        return context;
    }

    public static SmartHomeDbContext BuildSmartHomeDbContext()
    {
        DbContextOptions<SmartHomeDbContext> options = new DbContextOptionsBuilder<SmartHomeDbContext>()
            .UseSqlite(Connection)
            .Options;

        Connection.Open();

        var context = new SmartHomeDbContext(options);

        return context;
    }
}
