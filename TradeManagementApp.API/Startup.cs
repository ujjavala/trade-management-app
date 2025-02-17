using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TradeManagementApp.Persistence;
using Microsoft.EntityFrameworkCore;
using TradeManagementApp.Domain.Repositories;
using TradeManagementApp.Domain.Models;
using TradeManagementApp.Persistence.Repositories;
using TradeManagementApp.Application.Services;

namespace TradeManagementApp.API
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
            services.AddControllers();

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString(Constants.DefaultConnectionStringName));
            });

            services.AddScoped<ITradeService, TradeService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITradeRepository, TradeRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            // Register LruCache<string, Account> as a singleton
            services.AddSingleton(new LruCache<string, Account>(capacity: Constants.CacheCapacity));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Constants.SwaggerVersion, new OpenApiInfo { Title = Constants.SwaggerTitle, Version = Constants.SwaggerVersion });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint(Constants.SwaggerEndpointUrl, $"{Constants.SwaggerTitle} {Constants.SwaggerVersion}"));
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