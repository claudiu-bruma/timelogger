using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Timelogger.Core.DTOs;

namespace Timelogger.Core.Interfaces
{
    public interface ITimeLogService
    {
        Task AddTimeLog(AddTimeLogDto timeLog, CancellationToken cancellationToken);
        Task<IEnumerable<TimeLogDto>> GetLogsByProjectId(int projectId, CancellationToken cancellationToken);
    }
}