using ClimateBot.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ClimateBot.Services;
using ClimateBot.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add session support
builder.Services.AddDistributedMemoryCache(); // Utiliza memoria caché distribuida para almacenar sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Puedes ajustar esto según tus necesidades
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
