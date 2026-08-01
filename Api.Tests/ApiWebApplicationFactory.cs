namespace Api.Tests
{
  using System;
  using Domain.Enums;
  using Domain.Models;
  using Infrastructure.Database;
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.AspNetCore.Mvc.Testing;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;

  /// <summary>
  /// Boots the Api host against an in-memory database, seeded with a known user, for integration tests.
  /// </summary>
  public class ApiWebApplicationFactory : WebApplicationFactory<Program>
  {
    public const string KnownEmail = "known@example.com";

    public const string UnknownEmail = "unknown@example.com";

    public const string AdminEmail = "admin@example.com";

    public const string RestrictedEmail = "restricted@example.com";

    private readonly string databaseName = Guid.NewGuid().ToString();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
      builder.UseEnvironment("Testing");

      builder.ConfigureServices(services =>
      {
        services.AddDbContext<ModelDbContext>(options => options.UseInMemoryDatabase(this.databaseName));

        using ServiceProvider provider = services.BuildServiceProvider();
        using IServiceScope scope = provider.CreateScope();
        ModelDbContext dbContext = scope.ServiceProvider.GetRequiredService<ModelDbContext>();
        dbContext.Users.Add(new User("Known User", KnownEmail, UserRole.Operator));
        dbContext.Users.Add(new User("Admin User", AdminEmail, UserRole.Admin));
        dbContext.Users.Add(new User("Restricted User", RestrictedEmail, UserRole.User));
        dbContext.SaveChanges();
      });
    }
  }
}
