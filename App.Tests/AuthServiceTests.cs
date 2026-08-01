namespace App.Tests
{
  using System;
  using System.Threading.Tasks;
  using App.Services;
  using Domain.Enums;
  using Domain.Models;
  using Infrastructure.Database;
  using Microsoft.EntityFrameworkCore;
  using Xunit;

  public class AuthServiceTests
  {
    private static ModelDbContext CreateDbContext()
    {
      DbContextOptions<ModelDbContext> options = new DbContextOptionsBuilder<ModelDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

      return new ModelDbContext(options);
    }

    [Fact]
    public async Task FindUserByEmailAsync_KnownEmail_ReturnsUser()
    {
      using ModelDbContext dbContext = CreateDbContext();
      User user = new User("Jane", "jane@example.com", UserRole.Operator);
      dbContext.Users.Add(user);
      await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);

      AuthService service = new AuthService(dbContext);
      User? result = await service.FindUserByEmailAsync("jane@example.com");

      Assert.NotNull(result);
      Assert.Equal(user.UserId, result.UserId);
    }

    [Fact]
    public async Task FindUserByEmailAsync_UnknownEmail_ReturnsNull()
    {
      using ModelDbContext dbContext = CreateDbContext();

      AuthService service = new AuthService(dbContext);
      User? result = await service.FindUserByEmailAsync("missing@example.com");

      Assert.Null(result);
    }
  }
}
