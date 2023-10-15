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
    public class TimeLogRepository : ITimeLogRepository
    {
        protected readonly ITimeLoggerDbContext _context;
        protected readonly int _userId; 
        public TimeLogRepository(ITimeLoggerDbContext context, IIdentityService identityService)
        {
            
            _context = context;
            _userId = identityService.CurrentUserId;
        }
        public   async Task<IEnumerable<TimeLog>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.TimeLogs.Include(x=>x.Project).Where(x => x.UserId == _userId).ToListAsync(cancellationToken);
        }
        public async Task<TimeLog> GetByIdAsync(int id)
        {
            return await _context.TimeLogs.Where(x => x.UserId == _userId).FirstOrDefaultAsync(x => x.Id == id);
        } 
        public async Task AddAsync(TimeLog entity)
        {
            entity.UserId = _userId;
            await _context.TimeLogs.AddAsync(entity);
        }

        public void Update(TimeLog entity)
        {
            if (entity.UserId != _userId)
            {
                return;
            }
            _context.TimeLogs.Update(entity);
        }
        public async Task<IEnumerable<TimeLog>> GetLogsByProjectId(int projectId, CancellationToken cancellationToken)
        {
           return await _context.TimeLogs.Include(x => x.Project).Where(t => t.Project.Id == projectId).ToListAsync( cancellationToken);
        }
        public void Remove(TimeLog entity)
        {
            if (entity.UserId != _userId)
            {
                return;
            }
            _context.TimeLogs.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}