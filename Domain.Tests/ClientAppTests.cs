namespace Domain.Tests
{
  using System;
  using Domain.Models;
  using Xunit;

  public class ClientAppTests
  {
    [Fact]
    public void Constructor_AssignsClientIdAndProperties()
    {
      Guid createdBy = Guid.NewGuid();
      DateTimeOffset before = DateTimeOffset.UtcNow;

      ClientApp clientApp = new ClientApp("My App", "hash", "myapp.com", createdBy);

      Assert.NotEqual(Guid.Empty, clientApp.ClientId);
      Assert.Equal("My App", clientApp.Name);
      Assert.Equal("hash", clientApp.ClientSecret);
      Assert.Equal("myapp.com", clientApp.RedirectDomain);
      Assert.Equal(createdBy, clientApp.CreatedBy);
      Assert.InRange(clientApp.CreatedOn, before, DateTimeOffset.UtcNow);
    }
  }
}
