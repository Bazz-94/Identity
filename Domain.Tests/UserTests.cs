namespace Domain.Tests
{
  using System;
  using Domain.Enums;
  using Domain.Models;
  using Xunit;

  public class UserTests
  {
    [Fact]
    public void Constructor_AssignsUserIdAndProperties()
    {
      User user = new User("Jane", "jane@example.com", UserRole.Operator);

      Assert.NotEqual(Guid.Empty, user.UserId);
      Assert.Equal("Jane", user.UserName);
      Assert.Equal("jane@example.com", user.Email);
      Assert.Equal(UserRole.Operator, user.Role);
    }
  }
}
