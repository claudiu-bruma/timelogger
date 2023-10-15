using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;
using FluentAssertions;
using Timelogger.Api.Controllers;
using Timelogger.Core.DTOs;
using Timelogger.Core.Interfaces;

namespace Timelogger.Api.Tests.Controllers
{
    [TestFixture]
    public class TimelogsControllerTests
    {
        private Mock<ITimeLogService> _timelogServiceMock;
        private TimelogsController _timelogsController;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _timelogServiceMock = new Mock<ITimeLogService>();
            _timelogsController = new TimelogsController(_timelogServiceMock.Object);
            _fixture = new Fixture();
        }

        [Test]
        public async Task Get_ShouldReturn_AllTimeLogs()
        {
            var productId = _fixture.Create<int>();
            var timeLogDtos = _fixture.Build<TimeLogDto>()
                .With(x=>x.ProjectId , productId)
                .CreateMany(5);
            _timelogServiceMock.Setup(x => x.GetLogsByProjectId(productId ,It.IsAny<CancellationToken>()))
                .ReturnsAsync(timeLogDtos);


            var result = await _timelogsController.Get(productId ,CancellationToken.None);

            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().BeEquivalentTo(timeLogDtos);
        }

        [Test]
        public async Task Post_ValidTimeLog_CallsAddTimeLog()
        {
            var timeLog = _fixture.Build<AddTimeLogDto>()
                .Create();
            var result = await _timelogsController.Post(timeLog, CancellationToken.None);
            _timelogServiceMock.Verify(x => x.AddTimeLog(timeLog, It.IsAny<CancellationToken>()), Times.Once);
            result.Should().BeOfType<OkResult>(); 

        }

        [Test]
        public async Task Post_TimeLogIsNull_ReturnsBadRequest()
        {
            var result = await _timelogsController.Post(null, CancellationToken.None);
            _timelogServiceMock.Verify(x => x.AddTimeLog(It.IsAny<AddTimeLogDto>(), It.IsAny<CancellationToken>()), Times.Never);
            result.Should().BeOfType<BadRequestResult>();

        }

    }
}