using Microsoft.EntityFrameworkCore;

namespace UserService.Extensions
{
    public static class MigrationsExtensions
    {
        public static void MigrateDatabase<TContext>(this IHost host) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<TContext>();
            int retries = 5;
            while (retries > 0)
            {
                try
                {
                    db.Database.Migrate();
                    return;
                }
                catch
                {
                    Thread.Sleep(5000);
                    retries--;
                }
            }
        }
    }
}
