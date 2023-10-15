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
    public class ProjectsService : IProjectsService
    {
        private readonly IProjectRepository _projectRepository; 

        public ProjectsService(IProjectRepository projectRepository )
        {
            _projectRepository = projectRepository; 
        }

        public async Task AddProject(AddProjectDto project, CancellationToken cancellationToken)
        {
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project), "Project is null");
            }

            if (string.IsNullOrEmpty(project.Name))
            {
                throw new ArgumentException("Project needs a name", nameof(project.Name));
            }

            var projectEntity = new Project()
            {
                Deadline = project.Deadline,
                Name = project.Name,
                IsCompleted = project.IsCompleted,
                TimeLogs = new List<TimeLog>()
            };
            await _projectRepository.AddAsync(projectEntity);
            await _projectRepository.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjects(CancellationToken cancellationToken)
        {
            var projects = (await _projectRepository.GetAllAsync(cancellationToken)).ToList();
            if (projects is null || !projects.Any())
            {
                return  new List<ProjectDto>();
            }

            return projects.Select(x=>new ProjectDto()
            {
                Deadline = x.Deadline,
                Id = x.Id,
                IsCompleted = x.IsCompleted,
                Name = x.Name,
                TimeLogs =(x.TimeLogs is null) ? new List<TimeLogDto>()
                    : x.TimeLogs.Select(t=>new TimeLogDto()	 
                {
                    Comment = t.Comment,
                    Id = t.Id,
                    LogDate = t.LogDate,
                    LogTimeInMinutes = t.LogTimeInMinutes
                }).ToList()
            });
            
        }

        public async Task CompleteProject(int id, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            project.IsCompleted = true;
            _projectRepository.Update(project);
            await _projectRepository.SaveChangesAsync(cancellationToken);
        }
    }
}