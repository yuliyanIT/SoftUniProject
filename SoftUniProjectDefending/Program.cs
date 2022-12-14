using FaceitRankChecker.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();



builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "FaceitAPI",
    pattern: "{controller=FaceitAPI}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "FaceitCallback",
    pattern: "{controller=FaceitCallback}/{action=Index}/{code?}");
//pattern: "FaceitCallback/{code?}",
//defaults: new { controller = "FaceitCallbackController", action = "FaceitCallback" });
app.MapControllerRoute(
    name: "EloOverlay",
    pattern: "{controller=EloOverlay}/{action=Index}/{nickname?}");

app.MapControllerRoute(
    name: "Stats",
    pattern: "{controller=StatsController}/{action=Index}/{code?}");

app.MapRazorPages();

app.Run();
