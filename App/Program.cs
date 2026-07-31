namespace App
{
  using System;
  using Api;
  using Client;
  using Microsoft.AspNetCore.Builder;
  using Microsoft.EntityFrameworkCore;
  using Microsoft.Extensions.Configuration;
  using Microsoft.Extensions.DependencyInjection;
  using Microsoft.Extensions.Hosting;
  using Migrations;

  public class Program
  {
    public static void Main(string[] args)
    {
      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      // Add services from the libraries to the container.
      builder.Services.AddApi();
      builder.Services.AddClient();
      builder.Services.AddOpenApi();

      string? connection = String.Empty;
      if (builder.Environment.IsDevelopment())
      {
        builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
        connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
      }
      else
      {
        connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
      }

      builder.Services.AddDbContext<ModelDbContext>(options => options.UseSqlServer(connection));

      WebApplication app = builder.Build();

      // Configure the HTTP request pipeline.
      if (!app.Environment.IsDevelopment())
      {
        app.MapOpenApi();
        app.UseSwaggerUI(options =>
        {
          options.SwaggerEndpoint("/openapi/v1.json", "v1");
        });
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      // Map the endpoints exposed by each library.
      app.MapApi();
      app.MapClient();

      app.Run();
    }
  }
}
