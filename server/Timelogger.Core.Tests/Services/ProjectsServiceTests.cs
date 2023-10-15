using AutoFixture;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using FluentAssertions;
using Timelogger.Core.DTOs;
using Timelogger.Core.Entities;
using Timelogger.Core.Interfaces;
using Timelogger.Core.Services;

namespace Timelogger.Core.Tests.Services
{
    [TestFixture]
    public class ProjectsServiceTests
    {
        private Mock<IProjectRepository> _projectRepositoryMock;
        private ProjectsService _projectsService;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _projectsService = new ProjectsService(_projectRepositoryMock.Object);
            _fixture = new Fixture();
        }

        [Test]
        public void AddProject_NullProject_ThrowsArgumentNullException()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _projectsService.AddProject(null, CancellationToken.None));
        }

        [Test]
        public void AddProject_EmptyName_ThrowsArgumentException()
        {
            var projectDto = _fixture.Create<AddProjectDto>();
            projectDto.Name = string.Empty;

            Assert.ThrowsAsync<ArgumentException>(() => _projectsService.AddProject(projectDto, CancellationToken.None));
        }

        [Test]
        public async Task AddProject_ValidProject_AddsProject()
        {
            var projectDto = _fixture.Create<AddProjectDto>();

            await _projectsService.AddProject(projectDto, CancellationToken.None);

            _projectRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<Project>()), Times.Once);
            _projectRepositoryMock.Verify(pr => pr.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task GetAllProjects_NoProjects_ReturnsEmptyList()
        {
            _projectRepositoryMock.Setup(pr => pr.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(new List<Project>());

            var result = await _projectsService.GetAllProjects(CancellationToken.None);

            result.Should().BeEmpty();
        }

        [Test]
        public async Task GetAllProjects_ThereAreProjects_ReturnsEmptyList()
        {
            var project = _fixture
                .Build<Project>()
                .With(x => x.TimeLogs, new List<TimeLog>())
                      .Create();
            var timelog = _fixture
                .Build<TimeLog>()
                .With(x => x.Project, project)
                .Create();
            project.TimeLogs.Add(timelog);

            _projectRepositoryMock.Setup(pr => pr.GetAllAsync(CancellationToken.None))
                .ReturnsAsync(new List<Project>() { project });

            var result = await _projectsService.GetAllProjects(CancellationToken.None);

            result.Should().NotBeEmpty();
            result.Should().Contain(x => x.Id == project.Id);
            result.Should().Contain(x => x.Name == project.Name);
            result.Should().Contain(x => x.Deadline == project.Deadline);
            result.Should().Contain(x => x.IsCompleted == project.IsCompleted);
            result.Should().Contain(x => x.TimeLogs.Count == project.TimeLogs.Count);
        }


        [Test]
        public async Task CompleteProject_ExistingProject_SetsTheProjectToCompleted()
        {
            var project = _fixture.Build<Project>()
                .With(x => x.TimeLogs, new List<TimeLog>())
                .With(x => x.IsCompleted, false)
                .Create();
            _projectRepositoryMock.Setup(pr => pr.GetByIdAsync(project.Id))
                .ReturnsAsync(project);

            await _projectsService.CompleteProject(project.Id, CancellationToken.None);

            project.IsCompleted.Should().BeTrue();
            _projectRepositoryMock.Verify(pr => pr.Update(It.Is<Project>(p => p.IsCompleted)), Times.Once);
        }

    }
}