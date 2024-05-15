using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testCosmosdb.Data.Core;
using testCosmosdb.Data.DTO;
using testCosmosdb.Data.Inteface;

namespace testCosmosdb.Data.Abstract
{
    public class ProgramRepository : IProgramRepository
    {

        private readonly Container _userContainer;
        private readonly Container _programApplicationContainer;
        private readonly Container _programContainer;
        private readonly IConfiguration configuration;

        public ProgramRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            this.configuration = configuration;
            var databaseName = configuration["CosmosDbSettings:DatabaseName"];
            var userContainerName = "User";
            var programContainerName = "Program";
            var programApplicationContainerName = "ProgramApplication";
            _userContainer = cosmosClient.GetContainer(databaseName, userContainerName);
            _programApplicationContainer = cosmosClient.GetContainer(databaseName, programApplicationContainerName);
            _programContainer = cosmosClient.GetContainer(databaseName, programContainerName);
        }

        public async Task<ProgramApplicationDTO> ApplyProgramAsync(ProgramApplication programapplication)
        {
           

            // Fetch related data from other containers
            var user = await GetUserByIdAsync(programapplication.UserId);
            var program = await GetProgramByIdAsync(programapplication.ProgramId);

            var response = await _programApplicationContainer.CreateItemAsync(programapplication);
            var data = response.Resource;


            return new ProgramApplicationDTO
            {
                Id = data.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                Nationality = user.Nationality,
                IdNumber = user.IdNumber,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                Description = program.Description,
                Title = program.Title
            };
        }

        public async Task<Program> CreateProgramAsync(Program program)
        {
            var response = await _programContainer.CreateItemAsync(program);
            return response.Resource;
        }


        public async Task<Core.User> GetUserByIdAsync(string userId)
        {
            var query = new QueryDefinition("SELECT * FROM User u WHERE u.id = @userId")
                .WithParameter("@userId", userId);
            var iterator = _userContainer.GetItemQueryIterator<Core.User>(query);
            var user = (await iterator.ReadNextAsync()).FirstOrDefault();
            return user;
        }

        public async Task<Program> GetProgramByIdAsync(string programId)
        {
            var query = new QueryDefinition("SELECT * FROM Program p WHERE p.id = @programId")
                .WithParameter("@programId", programId);
            var iterator = _programContainer.GetItemQueryIterator<Program>(query);
            var program = (await iterator.ReadNextAsync()).FirstOrDefault();
            return program;
        }
    }
}
