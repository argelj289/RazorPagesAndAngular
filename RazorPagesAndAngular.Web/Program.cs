using Microsoft.Extensions.Configuration;
using System.Security.Principal;
using Microsoft.Identity.Web.UI;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace RazorPagesAndAngular.Web
{
    public class Program
    {
        public static IConfiguration Configuration { get; private set; }
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Configuration = builder.Configuration;


            /* Add services to the container. */

            //builder.Services.AddHttpContextAccessor();

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                // Handling SameSite cookie according to https://docs.microsoft.com/en-us/aspnet/core/security/samesite?view=aspnetcore-3.1
                options.HandleSameSiteCookieCompatibility();
            });


            //builder.Services.AddControllersWithViews();

            builder.Services.AddRazorPages(options =>
            {
                // Redirect to index page for all routes where Angular scripts are loaded (SPA)
                options.Conventions.AddPageRoute("/index", "{*url}");
            }).AddMicrosoftIdentityUI();

            //builder.Services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);


            var app = builder.Build();

            /* Configure the HTTP request pipeline. */
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            //app.UseAuthentication();

            //app.Use((context, next) =>
            //{
            //    Thread.CurrentPrincipal = context.User;
            //    return next(context);
            //});

            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    // MSAL routes
            //    endpoints.MapControllerRoute(
            //        name: "SigninOidc", // Route name
            //        pattern: "signin-oidc");

            //    endpoints.MapControllerRoute(
            //        name: "SignoutCallbackOidc", // Route name
            //        pattern: "signout-callback-oidc");

            //    endpoints.MapControllers();
            //});

            app.MapRazorPages();

            app.Run();
        }
    }
}