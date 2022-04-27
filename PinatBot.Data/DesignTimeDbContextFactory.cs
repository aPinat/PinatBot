using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PinatBot.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<Database>
{
    public Database CreateDbContext(string[] args) =>
        new(new DbContextOptionsBuilder()
            .UseNpgsql("Host=localhost;Port=5500;Database=pinatbot;Username=pinatbot;Password=pinatbot")
            .UseSnakeCaseNamingConvention()
            .Options);
}
