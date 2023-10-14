using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Timelogger.Core.Entities;

namespace Timelogger.Core.Interfaces
{
    public interface ITimeLogRepository : IRepository<TimeLog>
    {
        Task<IEnumerable<TimeLog>> GetLogsByProjectId(int projectId, CancellationToken cancellationToken);

    }
}