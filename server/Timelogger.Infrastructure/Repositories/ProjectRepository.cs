using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Timelogger.Core.Entities;
using Timelogger.Core.Interfaces;
using Timelogger.Infrastructure.DbContext;

namespace Timelogger.Infrastructure.Repositories
{
    public class ProjectRepository :  IProjectRepository
    {
        protected readonly ITimeLoggerDbContext _context;
        protected readonly int _userId;
        public ProjectRepository(ITimeLoggerDbContext context, IIdentityService identityService)
        {
            _context = context; 
            _userId = identityService.CurrentUserId;
        }
        public   async Task<IEnumerable<Project>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Projects.Include(x=>x.TimeLogs).Where(x => x.UserId == _userId).ToListAsync(cancellationToken);
        }
        public async Task<Project> GetByIdAsync(int id)
        {
            return await _context.Projects.Where(x=>x.UserId == _userId).FirstOrDefaultAsync(x=>x.Id == id);
        }
 

        public async Task AddAsync(Project entity)
        {
            entity.UserId = _userId;
            await _context.Projects.AddAsync(entity);
        }

        public void Update(Project entity)
        {
            if (entity.UserId != _userId)
            {
                return;
            }
            _context.Projects.Update(entity);
        }
        public void Remove(Project entity)
        {
            if (entity.UserId != _userId)
            {
                return;
            }
            _context.Projects.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}