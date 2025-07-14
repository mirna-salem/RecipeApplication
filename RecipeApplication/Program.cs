using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using RecipeApplication.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Get connection string from configuration or environment variable
var connectionString = builder.Configuration.GetConnectionString("RecipeApplicationDbContextConnection") 
    ?? Environment.GetEnvironmentVariable("CONNECTION_STRING") 
    ?? throw new InvalidOperationException("Connection string 'RecipeApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<RecipeApplicationDbContext>(options =>
	options.UseSqlServer(connectionString,
		sqlServerOptionsAction: sqlOptions =>
		{
			sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
		}));


builder.Services.AddDefaultIdentity<RecipeApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<RecipeApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

// HTTPS port configuration
builder.Services.Configure<Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionOptions>(options =>
{
    options.HttpsPort = 443; // Specify the desired HTTPS port
});

// Cookie configuration
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.HttpOnly = HttpOnlyPolicy.Always;
    options.Secure = CookieSecurePolicy.Always; // Set cookies as secure
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Recipe/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Recipe}/{action=Index}/{id?}"
);

app.MapRazorPages();
app.Run();
