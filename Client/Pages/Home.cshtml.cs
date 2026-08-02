namespace Client.Pages
{
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Mvc.RazorPages;

  /// <summary>
  /// Authenticated landing page linking to the management sections available to the current user.
  /// </summary>
  [Authorize]
  public class HomeModel : PageModel
  {
    /// <summary>
    /// Renders the page; section links are shown or hidden based on the current user's role.
    /// </summary>
    public void OnGet()
    {
    }
  }
}
