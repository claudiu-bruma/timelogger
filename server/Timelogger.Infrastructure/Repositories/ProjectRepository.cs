using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Timelogger.Core.Entities;
using Timelogger.Core.Interfaces;
using Timelogger.Infrastructure.DbContext;

namespace Timelogger.Infrastructure.Repositories
{
    public class ProjectRepository : Repository<Project>, IProjectRepository
    {
        public ProjectRepository(ITimeLoggerDbContext context) : base(context)
        {

        }
        public new async Task<IEnumerable<Project>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Projects.Include(x=>x.TimeLogs).ToListAsync(cancellationToken);
        }
    }
}