using Microsoft.EntityFrameworkCore;
using Milky.DataAccess.Data;
using Milky.DataAccess.Repository;
using Milky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Milky.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Milky.Models;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>  //adding entity framework core to the project -db configuration
//Services.AddDbContext<ApplicationDbContext>This line registers the applicationdbcontext as a service in the application's service
//container.



 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  //getting permissions to add sql server

//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();
//options.UseSqlServer(...) - This part configures Entity Framework Core to use SQL Server as the database provider.
//builder.Configuration.GetConnectionString("DefaultConnection") - This part retrieves the connection string named "DefaultConnection" 
// from the application's configuration.

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    options.Cookie.Name = "YourCookieName"; // Optionally set a custom cookie name
});

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IEmailSender, EmailSender>();
// Add session support
builder.Services.AddSession();
// Add TempData configuration
builder.Services.AddMvc().AddSessionStateTempDataProvider();
builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Use session middleware
app.UseSession();

//Set culture settings
var cultureInfo = new CultureInfo("en-US");
cultureInfo.NumberFormat.CurrencySymbol ="₹";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
