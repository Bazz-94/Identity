namespace Domain.Enums
{
  /// <summary>
  /// Access level assigned to a <see cref="Domain.Models.User"/>.
  /// </summary>
  public enum UserRole
  {
    /// <summary>
    /// Regular user with no elevated access.
    /// </summary>
    User,

    /// <summary>
    /// Operator with access to operational admin pages.
    /// </summary>
    Operator,

    /// <summary>
    /// Administrator with full access.
    /// </summary>
    Admin,
  }
}
