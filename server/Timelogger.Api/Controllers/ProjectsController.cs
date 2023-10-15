using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Timelogger.Core.DTOs;
using Timelogger.Core.Interfaces;

namespace Timelogger.Api.Controllers
{
    [Route("api/projects")]
    public class ProjectsController : Controller
    {
        private IProjectsService _projectsService;

        public ProjectsController(IProjectsService projectsService)
        {
            _projectsService = projectsService;
        }

        [HttpGet]
        [Route("hello-world")]
        public string HelloWorld()
        {
            return "Hello Back!";
        }

        // GET api/projects
        [HttpGet]
        //route get all projects    
        [Route("", Name = "GetAll")]
        public async Task<IActionResult> GetAllProjects(CancellationToken cancellationToken)
        {
            var project =await _projectsService.GetAllProjects(cancellationToken);
            return Ok(project);
        } 

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddProjectDto project, CancellationToken cancellationToken)
        {
            if (project is null)
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(project.Name))
            {
                return BadRequest();
            }

            await _projectsService.AddProject(project, cancellationToken);

            //return created at route	
            return new StatusCodeResult(201);
        }
    }
}
