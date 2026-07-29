namespace Api
{
  using Microsoft.AspNetCore.Builder;
  using Microsoft.AspNetCore.Routing;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;

  /// <summary>
  /// Entry points used by the App host to compose the Api library into the request pipeline.
  /// </summary>
  public static class ApiExtensions
  {
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
      services.AddControllers();
      // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
      services.AddOpenApi();

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
