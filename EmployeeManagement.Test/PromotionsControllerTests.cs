using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Models;
using EmployeeManagement.Services.Test;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmployeeManagement.Test
{
    public class PromotionsControllerTests
    {
        [Fact]
        public async Task CreatePromotion_RequestPromotionForEligibleForEmployee_MustPromoteEmployee()
        {
            //Arrange
            var expectedEmployeeId = Guid.NewGuid();
            var currentJobLevel = 1;
            var promotionForCreationDto = new PromotionForCreationDto()
            {
                EmployeeId = expectedEmployeeId
            };

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(m => m.FetchInternalEmployeeAsync(It.IsAny<Guid>()))
                .ReturnsAsync(
                    new InternalEmployee("Kanan", "Garazada", 3, 3400, true, currentJobLevel)
                    {
                        Id = expectedEmployeeId,
                        SuggestedBonus = 500
                    });

            var eligibleForPromotionHandlerMock = new Mock<HttpMessageHandler>();
            eligibleForPromotionHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(
                        JsonSerializer.Serialize(
                            new PromotionEligibility() { EligibleForPromotion = true},
                            new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                            }),
                        Encoding.ASCII,
                        "application/json")
                });

            var httpClient = new HttpClient(eligibleForPromotionHandlerMock.Object);
            var promotionService = new PromotionService(httpClient,
                new EmployeeManagementTestDataRepository());

            var promotionsController = new PromotionsController(
                employeeServiceMock.Object, promotionService);

            //Act 
            var result = await promotionsController.CreatePromotion(promotionForCreationDto);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var promotionResultDto = Assert.IsType<PromotionResultDto>(okObjectResult.Value);
            Assert.Equal(expectedEmployeeId, promotionResultDto.EmployeeId);
            Assert.Equal(++currentJobLevel, promotionResultDto.JobLevel);
        }
    }
}
