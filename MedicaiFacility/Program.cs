using MedicaiFacility.Service;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Đăng ký Database và Configuration
builder.Services.AddDatabaseAndConfiguration(builder.Configuration);
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddServices();

// Cấu hình Authentication với Cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login";
        options.LogoutPath = "/Users/Logout";
        options.AccessDeniedPath = "/Users/AccessDenied";
    });

// Cấu hình Authorization (nếu cần)
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Thêm UseAuthentication trước UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();