using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Timelogger.Core.DTOs;

namespace Timelogger.Core.Interfaces
{
    public interface IProjectsService
    {
        Task AddProject(AddProjectDto project, CancellationToken cancellationToken);
        Task<IEnumerable<ProjectDto>> GetAllProjects(CancellationToken cancellationToken);
        Task CompleteProject(int id, CancellationToken cancellationToken);
    }
}