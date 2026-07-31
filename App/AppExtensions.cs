namespace App
{
  using System;
  using Infrastructure.Database;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;

  /// <summary>
  /// Entry points used by the Api host to compose cross-cutting application services into the container.
  /// </summary>
  public static class AppExtensions
  {
    public static IServiceCollection AddApp(this IServiceCollection services, ConfigurationManager configuration, IHostEnvironment environment)
    {
      string? connection;
      if (environment.IsDevelopment())
      {
        configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
        connection = configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
      }
      else
      {
        connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
      }

      services.AddDbContext<ModelDbContext>(options => options.UseSqlServer(connection));

      return services;
    }
  }
}
