namespace Api
{
  using Microsoft.AspNetCore.Authentication.Cookies;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Routing;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;

  /// <summary>
  /// Entry points used by Program.cs to compose the Api library's own services into the request pipeline.
  /// </summary>
  public static class ApiExtensions
  {
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
      services.AddControllers();
      // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
      services.AddOpenApi();

      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
          options.LoginPath = "/";
          options.AccessDeniedPath = "/AccessDenied";
        });
      services.AddAuthorization();

      return services;
    }

    public static WebApplication MapApi(this WebApplication app)
    {
      if (app.Environment.IsDevelopment())
      {
        app.MapOpenApi();
      }

      app.MapControllers();

      return app;
    }
  }
}
