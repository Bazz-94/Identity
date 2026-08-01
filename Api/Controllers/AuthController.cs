namespace Api.Controllers
{
  using System.Collections.Generic;
  using System.Security.Claims;
  using System.Threading.Tasks;
  using App.Services;
  using Domain.Models;
  using Microsoft.AspNetCore.Authentication;
  using Microsoft.AspNetCore.Authentication.Cookies;
  using Microsoft.AspNetCore.Mvc;

  /// <summary>
  /// Dev login/logout endpoints. Signs in a user by email match, standing in for real Google SSO until sa-02.
  /// </summary>
  [ApiController]
  [Route("api/auth")]
  public class AuthController : ControllerBase
  {
    private readonly AuthService authService;

    public AuthController(AuthService authService)
    {
      this.authService = authService;
    }

    /// <summary>
    /// Signs in the user matching the given email, establishing a cookie session with their role.
    /// </summary>
    /// <param name="request">The login request carrying the email to match.</param>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
      User? user = await this.authService.FindUserByEmailAsync(request.Email);
      if (user is null)
      {
        return this.Unauthorized();
      }

      List<Claim> claims = new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
        new Claim(ClaimTypes.Role, user.Role.ToString()),
      };
      ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

      return this.Ok();
    }

    /// <summary>
    /// Signs the current user out, clearing the cookie session.
    /// </summary>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
      await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      return this.Ok();
    }
  }

  /// <summary>
  /// Request body for the dev login endpoint.
  /// </summary>
  /// <param name="Email">Email to match against a registered user.</param>
  public record LoginRequest(string Email);
}
