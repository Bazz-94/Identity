namespace App
{
  using System.Threading.Tasks;
  using Infrastructure.Database;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;

  /// <summary>
  /// Applies pending EF Core migrations to the configured database.
  /// </summary>
  public static class DatabaseMigrator
  {
    /// <summary>
    /// Applies any pending migrations.
    /// </summary>
    /// <param name="app">The built web application.</param>
    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
      using IServiceScope scope = app.Services.CreateScope();
      ModelDbContext dbContext = scope.ServiceProvider.GetRequiredService<ModelDbContext>();
      await dbContext.Database.MigrateAsync();
    }
  }
}
