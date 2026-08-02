namespace Client.Pages
{
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc.RazorPages;

  /// <summary>
  /// Client app registry management page. Admins have full CRUD; Operators have read-only access.
  /// </summary>
  [Authorize(Roles = "Admin,Operator")]
  public class ClientAppsModel : PageModel
  {
    /// <summary>
    /// Renders the page; the client app list itself is loaded client-side via the client-apps API.
    /// </summary>
    public void OnGet()
    {
    }
  }
}
