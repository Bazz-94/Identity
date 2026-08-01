namespace App.Tests
{
  using System;
  using System.Threading.Tasks;
  using App.Enums;
  using App.Services;
  using Domain.Models;
  using Infrastructure.Database;
  using Microsoft.EntityFrameworkCore;
  using Xunit;

  public class ClientRegistryServiceTests
  {
    private static ModelDbContext CreateDbContext()
    {
      DbContextOptions<ModelDbContext> options = new DbContextOptionsBuilder<ModelDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

      return new ModelDbContext(options);
    }

    [Fact]
    public async Task ValidateClientAsync_UnregisteredClientId_ReturnsUnknownClient()
    {
      using ModelDbContext dbContext = CreateDbContext();
      ClientRegistryService service = new ClientRegistryService(dbContext);

      ClientValidationResult result = await service.ValidateClientAsync(Guid.NewGuid(), "https://myapp.com/callback");

      Assert.Equal(ClientValidationResult.UnknownClient, result);
    }

    [Fact]
    public async Task ValidateClientAsync_RedirectUriHostDoesNotMatchDomain_ReturnsRedirectUriNotAllowed()
    {
      using ModelDbContext dbContext = CreateDbContext();
      User user = new User();
      ClientApp clientApp = new ClientApp("My App", "hash", "myapp.com", user.UserId);
      dbContext.Users.Add(user);
      dbContext.ClientApps.Add(clientApp);
      await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

      ClientRegistryService service = new ClientRegistryService(dbContext);
      ClientValidationResult result = await service.ValidateClientAsync(clientApp.ClientId, "https://evil.com/callback");

      Assert.Equal(ClientValidationResult.RedirectUriNotAllowed, result);
    }

    [Fact]
    public async Task ValidateClientAsync_RedirectUriHostMatchesDomain_ReturnsValid()
    {
      using ModelDbContext dbContext = CreateDbContext();
      User user = new User();
      ClientApp clientApp = new ClientApp("My App", "hash", "myapp.com", user.UserId);
      dbContext.Users.Add(user);
      dbContext.ClientApps.Add(clientApp);
      await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

      ClientRegistryService service = new ClientRegistryService(dbContext);
      ClientValidationResult result = await service.ValidateClientAsync(clientApp.ClientId, "https://myapp.com/callback");

      Assert.Equal(ClientValidationResult.Valid, result);
    }
  }
}
