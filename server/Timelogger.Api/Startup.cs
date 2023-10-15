using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting; 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting; 
using Timelogger.Core.Interfaces;
using Timelogger.Core.Services;
using Timelogger.Infrastructure.Authentication;
using Timelogger.Infrastructure.DbContext;
using Timelogger.Infrastructure.Configuration;
using Timelogger.Infrastructure.Repositories;
using Timelogger.Api.Middleware;

namespace Timelogger.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public IConfigurationRoot Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            _environment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.ConfigureDbContext();
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            services.AddScoped<IProjectsService, ProjectsService>();
            services.AddScoped<ITimeLogService, TimeLogService>();
            services.AddScoped<ITimeLoggerDbContext, TimeLoggerDbContext>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITimeLogRepository, TimeLogRepository>();
            services.AddScoped<IIdentityService, IdentityService>();

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.ConfigureSwaggerServices();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true)
                    .AllowCredentials());
            }

            app.UseMiddleware<UserIdExtractionMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();


            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.InitializeInMemoryData();
        }
    }
}