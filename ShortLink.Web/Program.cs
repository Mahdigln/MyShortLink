using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.WebEncoders.Testing;
using ShortLink.Infra.Data.Context;
using ShortLink.Infra.IoC;
using ShortLink.Web.Middelware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

#region IoC
builder.Services.RegisterService(builder.Configuration);

//this is for toastr notification
builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] {
    UnicodeRanges.BasicLatin,
    UnicodeRanges.Arabic
}));
#endregion

#region DB Context
builder.Services.AddDbContext<ShortLinkContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShortLinkSqlConnection"));
});
#endregion

#region authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/log-Out";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
});
#endregion



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

//app.UseShortLinkUrlRedirect();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

//app.Use(async (context, next) =>
//{
//    if (context.Request.Path.StartsWithSegments("/admin"))
//    {
//        if (!context.User.Identity.IsAuthenticated)
//        {
//            context.Response.Redirect("/login");
//        }
//        else if (!bool.Parse(context.User.FindFirstValue("IsAdmin")))
//        {
//            context.Response.Redirect("/login");
//        }
//    }


//    await next.Invoke();
//});

//app.MapFallbackToController("Index", "Home");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

