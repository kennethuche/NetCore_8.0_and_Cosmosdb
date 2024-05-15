
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Moq;
using System.ComponentModel;
using testCosmosdb.Data.Abstract;
using testCosmosdb.Data.Core;
using testCosmosdb.Data.Inteface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using testCosmosdb.Controllers;
using Xunit;

namespace testCosmosdb.test.Program
{
    public class ApplyProgramTest
    {

        


        [Fact]
        public async Task CreateProgram_ValidData_ReturnsCreated()
        {
            // Arrange
            var mockRepository = new Mock<IProgramRepository>();
            var mockMapper = new Mock<IMapper>();
            var controller = new ProgramController(mockRepository.Object, mockMapper.Object);
            var requestData = new ViewModel.ProgramVm();
            var responseData = new Data.Core.Program { Id = Guid.NewGuid().ToString() }; 

            mockMapper.Setup(m => m.Map<Data.Core.Program>(requestData)).Returns(responseData);
            mockRepository.Setup(r => r.CreateProgramAsync(responseData)).ReturnsAsync(responseData);

            // Act
            var result = await controller.CreateProgram(requestData);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(responseData.Id, createdResult.Value);
        }

       

        [Fact]
        public async Task CreateUser_ValidData_ReturnsOk()
        {
            // Arrange
            var mockRepository = new Mock<IProgramRepository>();
            var mockMapper = new Mock<IMapper>();
            var controller = new ProgramController(mockRepository.Object, mockMapper.Object);
            var requestData = new ViewModel.ProgramApplicationVm();
            //var responseData = true; // Example response data

            mockRepository.Setup(r => r.GetUserByIdAsync(requestData.UserId)).ReturnsAsync(new Data.Core.User()); // assuming GetUserByIdAsync returns a valid user
            mockRepository.Setup(r => r.GetProgramByIdAsync(requestData.ProgramId)).ReturnsAsync(new Data.Core.Program()); // assuming GetProgramByIdAsync returns a valid program
            mockMapper.Setup(m => m.Map<Data.Core.ProgramApplication>(requestData)).Returns(new Data.Core.ProgramApplication());
            mockRepository.Setup(r => r.ApplyProgramAsync(It.IsAny<Data.Core.ProgramApplication>())).ReturnsAsync(new Data.DTO.ProgramApplicationDTO());

            // Act
            var result = await controller.CreateUser(requestData);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(okResult.Value);
        }


    }


}
