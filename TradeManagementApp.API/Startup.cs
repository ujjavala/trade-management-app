using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TradeManagementApp.API.Services;
using TradeManagementApp.API.Services.Strategies;
using TradeManagementApp.Persistence.Repositories;

namespace TradeManagementApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<ITradeRepository, TradeRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ITradeService, TradeService>();
            services.AddScoped<IAccountService, AccountService>();

            // Register the strategies
            services.AddScoped<ITradeProcessingStrategy, StandardTradeProcessingStrategy>();
            // To use HighValueTradeProcessingStrategy, uncomment the following line and comment the above line
            // services.AddScoped<ITradeProcessingStrategy, HighValueTradeProcessingStrategy>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}