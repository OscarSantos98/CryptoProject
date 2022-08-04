using Blazored.LocalStorage;
using CryptoBlazor.Data;
using CryptoBlazor.Handlers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using System.Security.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

var appSettingSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingSection);

builder.Services.AddSingleton<HttpClient>();
builder.Services.AddHttpClient();

builder.Services.AddAuthorization();

builder.Services.AddSingleton<ChartService>();

builder.Services.AddTransient<ValidateHeaderHandler>();

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddHttpClient<IUserService, UserService>();

builder.Services.AddHttpClient<IThemeService, ThemeService>();


builder.Services.AddSingleton<ChartService>();



//builder.Services.AddHttpClient<ChartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();



app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
