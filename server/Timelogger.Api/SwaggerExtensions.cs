using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Timelogger.Api
{
    public static class SwaggerExtensions
    {
        public static void UseSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("v1/swagger.json", "Timelogger API V1"); });
        }

        public static void ConfigureSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "Timelogger API", Version = "v1" }); });
        }
    }
}