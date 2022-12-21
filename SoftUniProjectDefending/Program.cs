using FaceitRankChecker.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FaceitRankChecker.Hubs;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Authentication.OAuth;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

HttpClientHandler clientHandler = new HttpClientHandler();
clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

// Pass the handler to httpclient(from you are calling api)
HttpClient client = new HttpClient(clientHandler);


builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddResponseCompression(options =>
{
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
builder.Services.AddHttpClient();
builder.Services.AddAuthentication().AddOAuth("Faceit", options =>
{
    options.ClientId = "e9e58299-32c8-425d-9d12-0b61f4955774";
    options.ClientSecret = "ggmW0rmIgTXbZakY1wMU0jcRiquBYpPP9Vu1OzLb";
    options.CallbackPath = new PathString("/signin-faceit");
    options.AuthorizationEndpoint = "https://accounts.faceit.com/";

    options.AuthorizationEndpoint += "?response_type=code";
    options.AuthorizationEndpoint += $"&client_id={options.ClientId}";
    options.AuthorizationEndpoint += "&redirect_popup=true";
    options.TokenEndpoint = "https://www.faceit.com/en/oauth/token";
    options.UserInformationEndpoint = "https://www.faceit.com/en/oauth/me";
    options.Scope.Add("openid");
    options.SaveTokens = true;
   
}); 
//builder.Services.AddAuthentication().AddFaceit(options =>
//{
//    options.ClientId = Configuration["e9e58299-32c8-425d-9d12-0b61f4955774"];
//    options.ClientSecret = Configuration["ggmW0rmIgTXbZakY1wMU0jcRiquBYpPP9Vu1OzLb"];
//    options.Scope.Add("openid");
//    options.SaveTokens = true;
//}, "MyCustomAuthenticationScheme");




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
app.MapHub<ChatHub>("/chatHub");

app.Run();
