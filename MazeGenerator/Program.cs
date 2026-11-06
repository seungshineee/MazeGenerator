using MazeGenerator.Dtos;
using MazeGenerator.Models;
using MazeGenerator.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");

ArgumentNullException.ThrowIfNullOrEmpty(defaultConnection, nameof(defaultConnection));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(defaultConnection)
    .ConfigureWarnings(x => x.Ignore(RelationalEventId.PendingModelChangesWarning)));

builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IMazeGenerator, BacktrackerGenerator>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => options.LoginPath = "/Account/Login");

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.Migrate();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
