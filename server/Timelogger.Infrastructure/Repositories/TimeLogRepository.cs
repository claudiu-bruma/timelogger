using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Timelogger.Core.Entities;
using Timelogger.Core.Interfaces;
using Timelogger.Infrastructure.DbContext;

namespace Timelogger.Infrastructure.Repositories
{
    public class TimeLogRepository : Repository<TimeLog>, ITimeLogRepository
    {
        public TimeLogRepository(ITimeLoggerDbContext context) : base(context)
        {
            
        }
        public new async Task<IEnumerable<TimeLog>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.TimeLogs.Include(x=>x.Project).ToListAsync(cancellationToken);
        }
        public async Task<IEnumerable<TimeLog>> GetLogsByProjectId(int projectId, CancellationToken cancellationToken)
        {
           return await _context.TimeLogs.Include(x => x.Project).Where(t => t.Project.Id == projectId).ToListAsync( cancellationToken);
        }
    }
}