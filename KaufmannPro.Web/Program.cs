using KaufmannPro.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// ----------------- Datenbank -----------------
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// ----------------- Identity -----------------
builder.Services.AddIdentity<IdentityUser<int>, IdentityRole<int>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Login-Seite bleibt öffentlich
    options.LogoutPath = "/Account/Logout";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;
});

// ----------------- Razor Pages -----------------
builder.Services.AddRazorPages(options =>
{
    // Alle Seiten im Root-Ordner autorisieren, aber Login explizit erlauben
    options.Conventions.AuthorizeFolder("/");
    options.Conventions.AllowAnonymousToPage("/Account/Login");
});

var app = builder.Build();

// ----------------- Lokalisierung -----------------
var supportedCultures = new[] { new CultureInfo("de-DE") }; // nur Deutsch
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("de-DE"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};
app.UseRequestLocalization(localizationOptions);

// ----------------- Middleware -----------------
app.UseHttpsRedirection(); // zwingt HTTPS
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ----------------- Razor Pages Mapping -----------------
app.MapRazorPages(); // Login bleibt über AllowAnonymous erreichbar

// ----------------- Admin-User anlegen -----------------
using var scope = app.Services.CreateScope();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser<int>>>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

var admin = await userManager.FindByNameAsync("admin");
if (admin != null)
{
    await userManager.DeleteAsync(admin);
}

admin = new IdentityUser<int>
{
    UserName = "admin",
    NormalizedUserName = "ADMIN",
    Email = "admin@example.com",
    EmailConfirmed = true
};

var result = await userManager.CreateAsync(admin, "Admin123!");
if (!result.Succeeded)
{
    throw new Exception("Admin konnte nicht angelegt werden: " + string.Join(", ", result.Errors.Select(e => e.Description)));
}

if (!await roleManager.RoleExistsAsync("Admin"))
{
    await roleManager.CreateAsync(new IdentityRole<int> { Name = "Admin" });
}

await userManager.AddToRoleAsync(admin, "Admin");

Console.WriteLine("Admin-User erfolgreich neu angelegt.");

app.Run();
