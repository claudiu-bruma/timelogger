using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Timelogger.Api.Controllers;
using NUnit.Framework;
using Timelogger.Core.DTOs;
using Timelogger.Core.Interfaces;
using Timelogger.Core.Services;

namespace Timelogger.Api.Tests.Controllers
{
    [TestFixture]
    public class ProjectsControllerTests
    {
        private Mock<IProjectsService> _projectsServiceMock;
        private ProjectsController _projectsController;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _projectsServiceMock = new Mock<IProjectsService>();
            _projectsController = new ProjectsController(_projectsServiceMock.Object);
            _fixture = new Fixture();
        }
        [Test]
        public void HelloWorld_ShouldReply_HelloBack()
        {
            ProjectsController sut = new ProjectsController(null);

            string actual = sut.HelloWorld();

            Assert.AreEqual("Hello Back!", actual);
        }
        [Test]
        public async Task GetAllProjects_ShouldReturn_AllProjects()
        {
            var projects = _fixture.Build<ProjectDto>()
                .Without(x => x.TimeLogs)
                .CreateMany(5);
            _projectsServiceMock.Setup(x => x.GetAllProjects(It.IsAny<CancellationToken>()))
                .ReturnsAsync(projects);


            var result = await _projectsController.GetAllProjects(CancellationToken.None);

            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().BeEquivalentTo(projects);
        }

        [Test]
        public async Task Post_ValidProject_CallsAddProject()
        {
            var project = _fixture.Build<AddProjectDto>()
                .Create();
            var result = await _projectsController.Post(project, CancellationToken.None);
            _projectsServiceMock.Verify(x => x.AddProject(project, It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeOfType<StatusCodeResult>();
            result.As<StatusCodeResult>().StatusCode.Should().Be(201);

        }


        [Test]
        public async Task Post_ProjectHasNoName_ReturnsBadRequest()
        {
            var project = _fixture.Build<AddProjectDto>()
                .Without(x => x.Name)
                .Create();
            var result = await _projectsController.Post(project, CancellationToken.None);
            _projectsServiceMock.Verify(x => x.AddProject(project, It.IsAny<CancellationToken>()), Times.Never);
            result.Should().BeOfType<BadRequestResult>();

        }


        [Test]
        public async Task Post_ProjectIsNull_ReturnsBadRequest()
        {
            var result = await _projectsController.Post(null, CancellationToken.None);
            _projectsServiceMock.Verify(x => x.AddProject(It.IsAny<AddProjectDto>(), It.IsAny<CancellationToken>()), Times.Never);
            result.Should().BeOfType<BadRequestResult>();

        }
    }
}
