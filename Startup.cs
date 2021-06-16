using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Nemesys.Models;
using Nemesys.Models.Interfaces;
using Nemesys.Models.Repositories;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SixLabors.ImageSharp.Web.Processors;
using Nemesys.Services;
using System;

namespace Nemesys
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public IConfiguration _configuration;

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Sets up EF core
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("NemesysContextConnection"))
            );

            //Sets up Identity core
            services.AddDefaultIdentity<User>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            //Configures sessions and cookies
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
                options.SlidingExpiration = true;
            });

            //Image sharp used for loading images correctly
            services.AddImageSharp()
                .RemoveProcessor<FormatWebProcessor>()
                .RemoveProcessor<BackgroundColorWebProcessor>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation(); //Adds MVC capabilities            

            if (_env.IsDevelopment())
            {
                services.AddSingleton<INemesysRepository, MockNemesysRepository>();
            }
            else
            {
                services.AddTransient<INemesysRepository, SqlNemesysRepository>();
            }

            services.AddTransient<IEmailSender, EmailSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseImageSharp();

            app.UseHttpsRedirection(); //redirects HTTP:// urls to HTTPS:// ones

            //Allows access to static resources in the wwwroot folder
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24 * 365;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            }); 

            app.UseRouting(); //Allows routing URLs to specific endpoints (such as a controller)

            app.UseAuthentication();
            app.UseAuthorization();

            //Specifies which urls map to which endpoints
            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapRazorPages();
            });
        }
    }
}
