namespace Domain.Models
{
  using System;
  using Domain.Enums;

  /// <summary>
  /// A user of Identity, identified by email and assigned a role that gates access to admin pages.
  /// </summary>
  public class User
  {
    private User()
    {
    }

    /// <summary>
    /// The User class.
    /// </summary>
    /// <param name="userName">Display name of the user.</param>
    /// <param name="email">Email address used to match the user to their login identity.</param>
    /// <param name="role">Access level assigned to the user.</param>
    public User(string userName, string email, UserRole role)
    {
      this.UserId = Guid.NewGuid();
      this.UserName = userName;
      this.Email = email;
      this.Role = role;
    }

    /// <summary>
    /// System-generated identifier for the user.
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Display name of the user.
    /// </summary>
    public string UserName { get; private set; } = string.Empty;

    /// <summary>
    /// Email address used to match the user to their login identity.
    /// </summary>
    public string Email { get; private set; } = string.Empty;

    /// <summary>
    /// Access level assigned to the user.
    /// </summary>
    public UserRole Role { get; private set; }
  }
}
