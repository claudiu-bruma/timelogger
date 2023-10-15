using AutoFixture;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Timelogger.Core.DTOs;
using Timelogger.Core.Entities;
using Timelogger.Core.Interfaces;
using Timelogger.Core.Services;

namespace Timelogger.Core.Tests.Services
{
    [TestFixture]
    public class TimeLogServiceTests
    {
        private Mock<ITimeLogRepository> _timeLogRepositoryMock;

        private Mock<IProjectRepository> _projectRepositoryMock;
        private TimeLogService _timeLogService;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {

            _projectRepositoryMock = new Mock<IProjectRepository>();
            _timeLogRepositoryMock = new Mock<ITimeLogRepository>();
            _timeLogService = new TimeLogService(_timeLogRepositoryMock.Object, _projectRepositoryMock.Object);
            _fixture = new Fixture();
        }
        [Test]
        public void AddTimeLog_WhenProjectIdIs0_ShouldThrowException()
        {
            // Arrange
            var projectid = 0;
            var timeLog = _fixture.Build<AddTimeLogDto>()
                .With(x => x.ProjectId, projectid)
                .Create();
            _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectid)).ReturnsAsync((Project)null);


            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _timeLogService.AddTimeLog(timeLog, CancellationToken.None));
        }
        [Test]
        public void AddTimeLog_WhenProjectIdIsNull_ShouldThrowException()
        {
            // Arrange

            var timeLog = _fixture.Build<AddTimeLogDto>()
                .Without(x => x.ProjectId)
                .Create();

            // Assert
            Assert.ThrowsAsync<ArgumentNullException>(() => _timeLogService.AddTimeLog(timeLog, CancellationToken.None));
        }
        [Test]
        public void AddTimeLog_WhenProjectDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var projectid = _fixture.Create<int>();
            var timeLog = _fixture.Build<AddTimeLogDto>()
                                .With(x => x.ProjectId, projectid)
                .Create();
            _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectid)).ReturnsAsync((Project)null);


            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _timeLogService.AddTimeLog(timeLog, CancellationToken.None));
        }
        [Test]
        public void AddTimeLog_WhenProjectIsCompleted_ShouldThrowException()
        {
            // Arrange
            var projectid = _fixture.Create<int>();
            var timeLog = _fixture.Build<AddTimeLogDto>()
                .With(x => x.ProjectId, projectid)
                .Create();
            var project = _fixture.Build<Project>()
                .With(x => x.IsCompleted, true)
                .Without(x => x.TimeLogs)
                .With(x => x.Id, projectid)
                .Create();
            _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectid)).ReturnsAsync(project);


            // Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => _timeLogService.AddTimeLog(timeLog, CancellationToken.None));
        }
        [Test]
        public void AddTimeLog_WhenLoggedTimeIsLessThen30Mins_ShouldThrowException()
        {
            // Arrange
            var projectid = _fixture.Create<int>();
            var timeLog = _fixture.Build<AddTimeLogDto>()
                .With(x => x.ProjectId, projectid)
                .With(x => x.LogTimeInMinutes, 29)
                .Create();
            var project = _fixture.Build<Project>()
                .With(x => x.IsCompleted, false)
                .Without(x => x.TimeLogs)
                .With(x => x.Id, projectid)
                .Create();
            _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectid)).ReturnsAsync(project);


            // Assert
            Assert.ThrowsAsync<ArgumentException>(() => _timeLogService.AddTimeLog(timeLog, CancellationToken.None));
        }


        [Test]
        public async Task AddTimeLog_WhenTimelogIsValid_SavesTimelog()
        {
            // Arrange
            var projectid = _fixture.Create<int>();
            var timeLog = _fixture.Build<AddTimeLogDto>()
                .With(x => x.ProjectId, projectid)
                .With(x => x.LogTimeInMinutes, 31)
                .Create();
            var project = _fixture.Build<Project>()
                .With(x => x.IsCompleted, false)
                .Without(x => x.TimeLogs)
                .With(x => x.Id, projectid)
                .Create();
            _projectRepositoryMock.Setup(x => x.GetByIdAsync(projectid)).ReturnsAsync(project);


            // act
            await _timeLogService.AddTimeLog(timeLog, CancellationToken.None);

            //assert
            _timeLogRepositoryMock.Verify(x => x.AddAsync(It.Is<TimeLog>(y => y.LogTimeInMinutes == timeLog.LogTimeInMinutes
                                                                            && y.Comment == timeLog.Comment
                                                                            && y.LogDate == timeLog.LogDate)), Times.Once);
            _timeLogRepositoryMock.Verify(x=>x.SaveChangesAsync(CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task GetLogsByProjectId_ThereAreNoTimelogsForProject_NoTimeLogsAreRetunred()
        {
            // Arrange
            var projectid = _fixture.Create<int>();
            
            _timeLogRepositoryMock.Setup(x => x.GetLogsByProjectId(projectid, CancellationToken.None)).ReturnsAsync(null as List<TimeLog>);
            // Act 
            var timeLogs = await _timeLogService.GetLogsByProjectId(projectid, CancellationToken.None);

            // Assert
            timeLogs.Should().BeEmpty();

        }
        [Test]
        public async Task GetLogsByProjectId_ThereAreTimelogsForProject_TimeLogsAreRetunred()
        {
            // Arrange
            var projectid = _fixture.Create<int>();
            var timeLogs = _fixture.Build<TimeLog>()
                .With(x=>x.Project, 
                    _fixture.Build<Project>()
                    .With(y=>y.TimeLogs,new List<TimeLog>())
                    .With(y=>y.Id , projectid).Create())
                .Create();
                
            _timeLogRepositoryMock.Setup(
                    x => 
                        x.GetLogsByProjectId(projectid, CancellationToken.None))
                .ReturnsAsync(new List<TimeLog>(){ timeLogs});
            // Act 
            var timeLogDtos = (await _timeLogService.GetLogsByProjectId(projectid, CancellationToken.None)).ToList();

            // Assert
            timeLogDtos.Should().NotBeEmpty();
            timeLogDtos.Should().HaveCount(1);
            timeLogDtos.Should().Contain(x => x.Id == timeLogs.Id && x.LogTimeInMinutes == timeLogs.LogTimeInMinutes && x.Comment == timeLogs.Comment);
            timeLogDtos.Should().Contain(x => x.ProjectId == projectid);

        }


    }
}
