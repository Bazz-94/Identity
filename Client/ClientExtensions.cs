namespace Client
{
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Routing;
  using Microsoft.Extensions.DependencyInjection;

  /// <summary>
  /// Entry points used by the Api host to compose the Client library into the request pipeline.
  /// </summary>
  public static class ClientExtensions
  {
    public static IServiceCollection AddClient(this IServiceCollection services)
    {
      services.AddRazorPages();

      return services;
    }

    public static WebApplication MapClient(this WebApplication app)
    {
      app.MapStaticAssets();
      app.MapRazorPages()
         .WithStaticAssets();

      return app;
    }
  }
}
