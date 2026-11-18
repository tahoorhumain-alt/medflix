using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebHostStreaming.Torrent;

namespace WebHostStreaming
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = CreateHostBuilder(args).Build();

            using (var scope = app.Services.CreateScope())
            {
                var torrentAutoDownloader = scope.ServiceProvider.GetRequiredService<ITorrentAutoDownloader>();
                torrentAutoDownloader.StartAsync();
            }

            app.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", false)
                                .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options =>
                    {
                        options.Limits.MaxRequestBodySize = null;
                    });
                    
                    webBuilder.UseConfiguration(configuration);

                    // --- FIX FOR RENDER DEPLOYMENT ---
                    // Render assigns a random port via the PORT environment variable.
                    // If it's not found (e.g., running locally), it falls back to 5000.
                    var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
                    webBuilder.UseUrls($"http://0.0.0.0:{port}");
                    // ---------------------------------

                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
