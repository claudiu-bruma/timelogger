using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Timelogger.Core.DTOs;
using Timelogger.Core.Entities;
using Timelogger.Core.Interfaces;

namespace Timelogger.Core.Services
{
    public class TimeLogService : ITimeLogService
    {
        private readonly ITimeLogRepository _timeLogRepository;
        private readonly IProjectRepository _projectRepository; 

        public TimeLogService(ITimeLogRepository timeLogRepository, IProjectRepository projectRepository  )
        {
            _timeLogRepository = timeLogRepository;
            _projectRepository = projectRepository; 
        }

        public async Task AddTimeLog(AddTimeLogDto timeLog, CancellationToken cancellationToken)
        {
            if (timeLog.ProjectId is null || timeLog.ProjectId == 0)
            {
                throw new ArgumentNullException(nameof(timeLog.ProjectId), "ProjectId is null or invalid");
            }
            var project = await _projectRepository.GetByIdAsync(timeLog.ProjectId.Value);
            if (project is null)
            {
                throw new ArgumentException("ProjectId is not a valid project id", nameof(timeLog.ProjectId));
            }

            if (project.IsCompleted)
            {
                throw new InvalidOperationException("Project is already completed");
            }

            if (timeLog.LogTimeInMinutes < 30)
            {
                throw new ArgumentException("LogTimeInMinutes must be greater than 30", nameof(timeLog.LogTimeInMinutes));
            }

            var newTimeLogEntity = new TimeLog
            {
                Comment = timeLog.Comment,
                LogTimeInMinutes = timeLog.LogTimeInMinutes,
                LogDate = timeLog.LogDate,
                Project = project
            };

            await _timeLogRepository.AddAsync(newTimeLogEntity);
            await _timeLogRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<TimeLogDto>> GetLogsByProjectId(int projectId, CancellationToken cancellationToken)
        {
            var timeLogs = await _timeLogRepository.GetLogsByProjectId(projectId, cancellationToken);

            if (timeLogs is null)
            {
                return new List<TimeLogDto>();
            }

            return timeLogs.Select(x=>new TimeLogDto()
            {
                Id = x.Id,
                Comment = x.Comment,
                LogDate = x.LogDate,
                LogTimeInMinutes = x.LogTimeInMinutes,
                ProjectId = x.Project.Id
            });
        }
    }
}