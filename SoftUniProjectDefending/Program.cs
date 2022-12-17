using FaceitRankChecker.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddAuthentication
   (options =>
   {
       IConfigurationSection faceitAuthNSection =
       config.GetSection("Authentication:Faceit");
       var ClientId = faceitAuthNSection["e9e58299-32c8-425d-9d12-0b61f4955774"];
       var ClientSecret = faceitAuthNSection["ggmW0rmIgTXbZakY1wMU0jcRiquBYpPP9Vu1OzLb"];
   });

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
    name: "Matches",
    pattern: "{controller=MatchesController}/{action=Index}/{code?}");

app.MapControllerRoute(
    name: "SearchPlayer",
    pattern: "{controller=SearchPlayer}/{action=Index}/{code?}");

app.MapRazorPages();

app.Run();
