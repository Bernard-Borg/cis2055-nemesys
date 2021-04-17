using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nemesys.Models.Interfaces;
using Nemesys.Models.Repositories;

namespace cis2205_nemesys
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation(); //Adds MVC capabilities
            services.AddSingleton<IStarsRepository, MockStarsRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection(); //redirects HTTP:// urls to HTTPS:// ones
            app.UseStatusCodePages(); //Returns simple messages for errors (can customise later)
            app.UseStaticFiles(); //Allows access to static resources in the wwwroot folder

            app.UseRouting(); //Allows routing URLs to specific endpoints (such as a controller)

            //Specifies which urls map to which endpoints
            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
