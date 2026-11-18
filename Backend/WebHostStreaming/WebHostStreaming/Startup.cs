using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using WebHostStreaming.Middlewares;
using WebHostStreaming.StartupExtensions;

namespace WebHostStreaming
{
    public class Startup
    {
        const string ALLOW_SPECIFIC_ORIGIN = "AllowSpecificOrigin";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(ALLOW_SPECIFIC_ORIGIN,
                    builder => builder.WithOrigins("*").AllowAnyHeader()
                    .AllowAnyMethod());
            });

            services.AddControllers();
            services.AddProviders();
            services.AddConverters();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 1. Setup paths
            var webRoot = env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var homePath = Path.Combine(webRoot, "home");
            var backofficePath = Path.Combine(webRoot, "backoffice");

            // 2. Serve the React frontend from the 'home' folder at the root URL (/)
            if (Directory.Exists(homePath))
            {
                app.UseDefaultFiles(new DefaultFilesOptions
                {
                    FileProvider = new PhysicalFileProvider(homePath),
                    RequestPath = ""
                });

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(homePath),
                    RequestPath = ""
                });
            }

            // 3. Serve the Backoffice frontend from the 'backoffice' folder
            if (Directory.Exists(backofficePath))
            {
                app.UseDefaultFiles(new DefaultFilesOptions
                {
                    FileProvider = new PhysicalFileProvider(backofficePath),
                    RequestPath = "/backoffice"
                });

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(backofficePath),
                    RequestPath = "/backoffice"
                });
            }

            // 4. Fallback for any other static files
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(ALLOW_SPECIFIC_ORIGIN);

            app.UseAuthorization();

            app.UseMiddleware<ErrorLoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                // API routing
                endpoints.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Movies}/{action=Suggested}/{id?}");

                endpoints.MapGet("/ping", async context =>
                {
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync("pong");
                });

                // --- FIX APPLIED HERE ---
                // SPA Fallback: Point to the 'home' folder where index.html actually lives
                endpoints.MapFallbackToFile("home/index.html");
            });
        }
    }
}
