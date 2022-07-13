using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReservaEspectaculo.Data;
using ReservaEspectaculo.Models;

namespace ReservaEspectaculo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<MiContexto>(options => options.UseInMemoryDatabase(databaseName: "MiBase"));
            //services.AddDbContext<MiContexto>(options => options
            //.UseSqlServer(Configuration.GetConnectionString("ORTDBConnectionString"))
            //);
            //services.AddDbContext<MiContexto>(options => options.UseSqlite("filename=ReservaEspectaculo.db"));
            services.AddDbContext<MiContexto>(
                cfg =>
                {
                    cfg.UseSqlite("filename=ReservaEspectaculo.db").EnableSensitiveDataLogging();
                }
                );

            // para identificarse, asignar rol
            services.AddIdentity<Usuario, Rol>().AddEntityFrameworkStores<MiContexto>();

            // para login/logout
            services.PostConfigure<Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions>
                (IdentityConstants.ApplicationScheme, opciones =>
                {
                    opciones.LoginPath = "/Accounts/Login";
                    opciones.AccessDeniedPath = "/Accounts/AccesoDenegado";
                });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // para login/logout
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
