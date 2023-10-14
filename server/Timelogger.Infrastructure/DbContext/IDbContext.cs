using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Timelogger.Core.Entities;

namespace Timelogger.Infrastructure.DbContext
{
    public interface ITimeLoggerDbContext
    {
        DbSet<Project> Projects { get; set; }
        DbSet<TimeLog> TimeLogs { get; set; }
        int SaveChanges(); 
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}