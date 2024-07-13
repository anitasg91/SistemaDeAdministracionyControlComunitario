﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SAyCC.WaterSystem.Utilities;

namespace WaterSystem
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
            //services.AddControllersWithViews();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc(options =>
            {
                //Proteccion contra ataques de falsificación de solicitudes.
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                //Solicita conectarse solo a través de https.
                //options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddTransient<IGenerals, Generals>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(12);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();

            app.UseHttpsRedirection();
            app.UseHsts(options => options.MaxAge(days: 365).IncludeSubdomains());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXContentTypeOptions();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                  name: "ReceiveData",
                  template: "{controller=SessionReceiver}/{action=ReceiveData}/{IdUsuario}/{IdApp}");
            });








            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); // Página de error detallada en entorno de desarrollo
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); // Página de error en producción
                app.UseHsts(); // Redirección a HTTPS en producción
            }

            app.UseHttpsRedirection(); // Redirección a HTTPS
            app.UseStaticFiles(); // Servir archivos estáticos (por ejemplo, CSS, JS, imágenes)

            app.UseEndpointRouting(); // Middleware de enrutamiento
            app.UseEndpoint();

            //app.UseEndpoint(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //});













        }
    }
}
