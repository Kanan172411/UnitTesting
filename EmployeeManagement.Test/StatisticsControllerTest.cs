using AutoMapper;
using EmployeeManagement.Controllers;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;

namespace EmployeeManagement.Test
{
    public class StatisticsControllerTest
    {
        [Fact]
        public void GetStatistics_InputFromHttpConnectionFeature_MustReturnInputtedIps()
        {
            //Arrange
            var localIpAddress = IPAddress.Parse("111.111.111.111");
            var localPort = 5000;
            var remoteIpAddress = IPAddress.Parse("222.222.222.222");
            var remotePort = 8080;

            var featureCollectionMock = new Mock<IFeatureCollection>();
            featureCollectionMock.Setup(e => e.Get<IHttpConnectionFeature>())
                .Returns(new HttpConnectionFeature
                {
                    LocalIpAddress = localIpAddress,
                    LocalPort = localPort,
                    RemoteIpAddress = remoteIpAddress,
                    RemotePort = remotePort
                });

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(e => e.Features)
                .Returns(featureCollectionMock.Object);

            var mapperConfiguration = new MapperConfiguration(
                c => c.AddProfile<MapperProfiles.StatisticsProfile>());
            var mapper = new Mapper(mapperConfiguration);

            var statisticController = new StatisticsController(mapper);

            statisticController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = httpContextMock.Object
            };

            //Act
            var result = statisticController.GetStatistics();

            //Assert
            var actionResult = Assert.IsType<ActionResult<StatisticsDto>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var statisticsDto = Assert.IsType<StatisticsDto>(okObjectResult.Value);
            Assert.Equal(localIpAddress.ToString(), statisticsDto.LocalIpAddress);
            Assert.Equal(localPort, statisticsDto.LocalPort);
            Assert.Equal(remoteIpAddress.ToString(), statisticsDto.RemoteIpAddress);
            Assert.Equal(remotePort, statisticsDto.RemotePort);
        }
    }
}
