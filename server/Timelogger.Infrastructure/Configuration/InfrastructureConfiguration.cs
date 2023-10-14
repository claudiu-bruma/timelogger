using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Timelogger.Infrastructure.DbContext;

namespace Timelogger.Infrastructure.Configuration
{
    public static class InfrastructureConfigurationExtensions
    {
        public static void ConfigureDbContext(this IServiceCollection services)
        {
            services.AddDbContext<TimeLoggerDbContext>(opt => opt.UseInMemoryDatabase("e-conomic interview"));
        }

    }
}