using ClimateBot.Web.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ClimateBot.Services;
using ClimateBot.Web.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Services.AddLogging(config =>
{
    config.AddConfiguration(builder.Configuration.GetSection("Logging"));
    config.AddConsole();
    config.AddDebug();
});

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session support
builder.Services.AddDistributedMemoryCache(); // Uses distributed memory cache to store sessions
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // You can adjust this based on your needs
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register your configuration singleton
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Register your NewsService related configurations
builder.Services.AddSingleton<INewsServiceFactory, NewsServiceFactory>();
builder.Services.AddHttpClient<INewsService, NewsService>((serviceProvider, httpClient) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    httpClient.BaseAddress = new Uri($"{configuration["NewsAPI:BaseUrl"]}{configuration["NewsAPI:ApiKey"]}");
});

// Register the ClimateService
builder.Services.AddScoped<IClimateService, ClimateService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Make sure to call UseSession after UseRouting and before UseEndpoints
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
