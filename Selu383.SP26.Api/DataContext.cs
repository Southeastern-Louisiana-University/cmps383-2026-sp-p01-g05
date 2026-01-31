using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Selu383.SP26.Api;

/*public static async Task Main(string[] args)
{
    var host = CreateHostBuilder(args).Build();

    using (var scope = host.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<DataContext>();
        await db.Database.MigrateAsync();
    }

    host.Run();
}
EnsureCreatedAsync();*/
public class DataContext : DbContext
        {
            public DataContext(DbContextOptions<DataContext> options) : base(options)
            {
            }
        }

