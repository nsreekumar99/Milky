using Microsoft.EntityFrameworkCore;
using Milky.DataAccess.Data;
using Milky.DataAccess.Repository;
using Milky.DataAccess.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>  //adding entity framework core to the project -db configuration
//Services.AddDbContext<ApplicationDbContext>This line registers the applicationdbcontext as a service in the application's service
//container.



 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));  //getting permissions to add sql server
                                                                                         //options.UseSqlServer(...) - This part configures Entity Framework Core to use SQL Server as the database provider.
                                                                                         //builder.Configuration.GetConnectionString("DefaultConnection") - This part retrieves the connection string named "DefaultConnection" 
                                                                                         // from the application's configuration.

builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
