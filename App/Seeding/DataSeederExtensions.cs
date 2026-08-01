namespace App.Seeding
{
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using Infrastructure.Database;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.Extensions.DependencyInjection;

  /// <summary>
  /// Runs every registered <see cref="IDataSeeder"/>. Which seeders are registered, and for which environment, is decided at DI composition time (see `AddApp`), not here. Assumes the schema already exists — apply migrations separately first.
  /// </summary>
  public static class DataSeederExtensions
  {
    /// <summary>
    /// Runs every registered <see cref="IDataSeeder"/>.
    /// </summary>
    /// <param name="app">The built web application.</param>
    public static async Task SeedDataAsync(this WebApplication app)
    {
      using IServiceScope scope = app.Services.CreateScope();
      ModelDbContext dbContext = scope.ServiceProvider.GetRequiredService<ModelDbContext>();

      IEnumerable<IDataSeeder> seeders = scope.ServiceProvider.GetServices<IDataSeeder>();
      foreach (IDataSeeder seeder in seeders)
      {
        await seeder.SeedAsync(dbContext);
      }
    }
  }
}
