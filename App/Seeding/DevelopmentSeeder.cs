namespace App.Seeding
{
  using System.Threading.Tasks;
  using App.Helpers;
  using Domain.Enums;
  using Domain.Models;
  using Infrastructure.Database;
  using Microsoft.EntityFrameworkCore;

  /// <summary>
  /// Seeds a development-only client app registration.
  /// </summary>
  public class DevelopmentSeeder : IDataSeeder
  {
    private readonly ClientSecretHasher hasher;

    public DevelopmentSeeder(ClientSecretHasher hasher)
    {
      this.hasher = hasher;
    }

    /// <inheritdoc />
    public async Task SeedAsync(ModelDbContext dbContext)
    {
      if (await dbContext.ClientApps.AnyAsync())
      {
        return;
      }

      User systemUser = new User("system", "admin@localhost", UserRole.Admin);
      dbContext.Users.Add(systemUser);

      User operatorUser = new User("operator", "operator@localhost", UserRole.Operator);
      dbContext.Users.Add(operatorUser);

      ClientApp clientApp = new ClientApp(
        "Client",
        this.hasher.Hash("secret"),
        "localhost",
        systemUser.UserId);
      dbContext.ClientApps.Add(clientApp);

      await dbContext.SaveChangesAsync();
    }
  }
}
