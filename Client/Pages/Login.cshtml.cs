namespace Client.Pages
{
  using Microsoft.AspNetCore.Mvc;
  using Microsoft.AspNetCore.Mvc.RazorPages;

  /// <summary>
  /// Dev login page. Redirects an already-authenticated user straight to the client app registry.
  /// </summary>
  public class LoginModel : PageModel
  {
    /// <summary>
    /// Redirects to the client app registry if already logged in; otherwise renders the login form.
    /// </summary>
    public IActionResult OnGet()
    {
      if (this.User.Identity?.IsAuthenticated == true)
      {
        return this.RedirectToPage("/Home");
      }

      return this.Page();
    }
  }
}
