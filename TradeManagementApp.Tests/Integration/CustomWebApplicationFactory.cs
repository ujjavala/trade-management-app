using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TradeManagementApp.Persistence;
using Microsoft.Extensions.Configuration;
using System.IO;
using Xunit;
using System;
using Microsoft.Extensions.Logging;

namespace TradeManagementApp.Tests.Integration
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly string _dbName;
        private static bool _migrationsApplied = false;

        public CustomWebApplicationFactory()
        {
            _dbName = Guid.NewGuid().ToString() + ".db"; // Generate a unique database name
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                // Load appsettings.json
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                // Build the configuration
                IConfigurationRoot configuration = config.Build();

                // Override UseInMemoryDatabase setting
                configuration["UseInMemoryDatabase"] = "false";
            });

            builder.ConfigureServices(services =>
            {
                // Remove the in-memory database registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<DataContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add SQLite database
                services.AddDbContext<DataContext>(options =>
                {
                    options.UseSqlite($"Data Source={_dbName}"); // Use a unique database name
                });

                // Build the service provider
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database context
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<DataContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    // Ensure the database is deleted and created and migrate it
                    var dbPath = db.Database.GetDbConnection().DataSource;
                    if (File.Exists(dbPath))
                    {
                        try
                        {
                            File.Delete(dbPath);
                            logger.LogInformation($"Database file deleted: {dbPath}"); // Add logging
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"Error deleting database file: {dbPath}");
                            throw;
                        }
                    }
                    else
                    {
                        logger.LogInformation($"Database file not found: {dbPath}"); // Add logging
                    }

                    if (!_migrationsApplied)
                    {
                        db.Database.Migrate();
                        _migrationsApplied = true;
                    }

                    // Seed the database with test data (optional)
                    // You can add seed data here if needed
                }
            });
        }
    }
}