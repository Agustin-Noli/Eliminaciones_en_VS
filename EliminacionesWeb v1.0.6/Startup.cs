using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EliminacionesWeb.Models;
using Microsoft.EntityFrameworkCore;
using EliminacionesWeb.Helpers;

namespace EliminacionesWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            this.environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(IISDefaults.AuthenticationScheme);


            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = true;

            });

            services.AddHttpContextAccessor();

            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddCors(options => options.AddPolicy("AllowAll", builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

            //services.AddControllers();
            //services.AddSession(s => s.IdleTimeout = TimeSpan.FromMinutes(30));
            //services.AddDbContext<EliminacionesContext_Custom>(opt => opt.UseSqlServer(Configuration.GetConnectionString("EliminacionesDB")));

            string conn = this.GetConnectionString(environment, Configuration.GetSection("servidor").Value, Configuration.GetSection("ambiente").Value, Configuration.GetSection("BaseDeDatos").Value);

            services.AddDbContext<EliminacionesContext_Custom>(opt => opt.UseSqlServer(conn));

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
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors("AllowAll");
            app.UseAuthentication();
            //app.UseAuthorization();
            //app.UseSession();


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        private string GetConnectionString(IWebHostEnvironment env, string servidor, string ambiente, string baseDeDatos)
        {
            ContextDeliveryHelper _contextHelper = new ContextDeliveryHelper(env, servidor, ambiente, baseDeDatos);

            return _contextHelper.ConnString();

        }


    }
}
