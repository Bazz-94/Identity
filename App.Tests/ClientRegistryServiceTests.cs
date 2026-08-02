namespace App.Tests
{
  using System;
  using System.Collections.Generic;
  using System.Threading.Tasks;
  using App.Enums;
  using App.Helpers;
  using App.Services;
  using Domain.Enums;
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
      ClientRegistryService service = new ClientRegistryService(dbContext, new ClientSecretHasher());

      ClientValidationResult result = await service.ValidateClientAsync(Guid.NewGuid(), "https://myapp.com/callback");

      Assert.Equal(ClientValidationResult.UnknownClient, result);
    }

    [Fact]
    public async Task ValidateClientAsync_RedirectUriHostDoesNotMatchDomain_ReturnsRedirectUriNotAllowed()
    {
      using ModelDbContext dbContext = CreateDbContext();
      User user = new User("system", "system@localhost", UserRole.Admin);
      ClientApp clientApp = new ClientApp("My App", "hash", "myapp.com", user.UserId);
      dbContext.Users.Add(user);
      dbContext.ClientApps.Add(clientApp);
      await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

      ClientRegistryService service = new ClientRegistryService(dbContext, new ClientSecretHasher());
      ClientValidationResult result = await service.ValidateClientAsync(clientApp.ClientId, "https://evil.com/callback");

      Assert.Equal(ClientValidationResult.RedirectUriNotAllowed, result);
    }

    [Fact]
    public async Task ValidateClientAsync_RedirectUriHostMatchesDomain_ReturnsValid()
    {
      using ModelDbContext dbContext = CreateDbContext();
      User user = new User("system", "system@localhost", UserRole.Admin);
      ClientApp clientApp = new ClientApp("My App", "hash", "myapp.com", user.UserId);
      dbContext.Users.Add(user);
      dbContext.ClientApps.Add(clientApp);
      await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

      ClientRegistryService service = new ClientRegistryService(dbContext, new ClientSecretHasher());
      ClientValidationResult result = await service.ValidateClientAsync(clientApp.ClientId, "https://myapp.com/callback");

      Assert.Equal(ClientValidationResult.Valid, result);
    }

    [Fact]
    public async Task CreateClientAppAsync_PersistsClientApp_ReturnsPlaintextSecret()
    {
      using ModelDbContext dbContext = CreateDbContext();
      User user = new User("system", "system@localhost", UserRole.Admin);
      dbContext.Users.Add(user);
      await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

      ClientSecretHasher hasher = new ClientSecretHasher();
      ClientRegistryService service = new ClientRegistryService(dbContext, hasher);

      (ClientApp clientApp, string plaintextSecret) = await service.CreateClientAppAsync("My App", "myapp.com", user.UserId);

      Assert.NotEqual(Guid.Empty, clientApp.ClientId);
      Assert.NotEqual(plaintextSecret, clientApp.ClientSecret);
      Assert.True(hasher.Verify(plaintextSecret, clientApp.ClientSecret));
      Assert.Single(await dbContext.ClientApps.ToListAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task GetClientAppsAsync_ReturnsAllWithCreatorUser()
    {
      using ModelDbContext dbContext = CreateDbContext();
      User user = new User("system", "system@localhost", UserRole.Admin);
      ClientApp clientApp = new ClientApp("My App", "hash", "myapp.com", user.UserId);
      dbContext.Users.Add(user);
      dbContext.ClientApps.Add(clientApp);
      await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

      ClientRegistryService service = new ClientRegistryService(dbContext, new ClientSecretHasher());
      List<ClientApp> clientApps = await service.GetClientAppsAsync();

      ClientApp result = Assert.Single(clientApps);
      Assert.Equal("system", result.CreatedByUser?.UserName);
    }

    [Fact]
    public async Task UpdateClientAppAsync_ExistingClient_UpdatesNameAndRedirectDomain()
    {
      using ModelDbContext dbContext = CreateDbContext();
      User user = new User("system", "system@localhost", UserRole.Admin);
      ClientApp clientApp = new ClientApp("My App", "hash", "myapp.com", user.UserId);
      dbContext.Users.Add(user);
      dbContext.ClientApps.Add(clientApp);
      await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

      ClientRegistryService service = new ClientRegistryService(dbContext, new ClientSecretHasher());
      bool updated = await service.UpdateClientAppAsync(clientApp.ClientId, "Renamed App", "newdomain.com");

      Assert.True(updated);
      ClientApp? reloaded = await dbContext.ClientApps.SingleOrDefaultAsync(app => app.ClientId == clientApp.ClientId, TestContext.Current.CancellationToken);
      Assert.Equal("Renamed App", reloaded?.Name);
      Assert.Equal("newdomain.com", reloaded?.RedirectDomain);
    }

    [Fact]
    public async Task UpdateClientAppAsync_UnknownClient_ReturnsFalse()
    {
      using ModelDbContext dbContext = CreateDbContext();
      ClientRegistryService service = new ClientRegistryService(dbContext, new ClientSecretHasher());

      bool updated = await service.UpdateClientAppAsync(Guid.NewGuid(), "Renamed App", "newdomain.com");

      Assert.False(updated);
    }

    [Fact]
    public async Task DeleteClientAppAsync_ExistingClient_RemovesClientApp()
    {
      using ModelDbContext dbContext = CreateDbContext();
      User user = new User("system", "system@localhost", UserRole.Admin);
      ClientApp clientApp = new ClientApp("My App", "hash", "myapp.com", user.UserId);
      dbContext.Users.Add(user);
      dbContext.ClientApps.Add(clientApp);
      await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

      ClientRegistryService service = new ClientRegistryService(dbContext, new ClientSecretHasher());
      bool deleted = await service.DeleteClientAppAsync(clientApp.ClientId);

      Assert.True(deleted);
      Assert.Empty(await dbContext.ClientApps.ToListAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task DeleteClientAppAsync_UnknownClient_ReturnsFalse()
    {
      using ModelDbContext dbContext = CreateDbContext();
      ClientRegistryService service = new ClientRegistryService(dbContext, new ClientSecretHasher());

      bool deleted = await service.DeleteClientAppAsync(Guid.NewGuid());

      Assert.False(deleted);
    }
  }
}
