using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testCosmosdb.Data.Inteface;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using testCosmosdb.Data.Core;
using System.Collections.Generic;

namespace testCosmosdb.Data.Abstract
{
    public class UserRepository : IUserRepository
    {
        private readonly Container _userContainer;
        private readonly Container _questionTypeContainer;
        private readonly IConfiguration configuration;

        public UserRepository(CosmosClient cosmosClient, IConfiguration configuration)
        {
            this.configuration = configuration;
            var databaseName = configuration["CosmosDbSettings:DatabaseName"];
            var userContainerName = "User";
            var questionTypeContainerName = "QuestionTypes";
            _userContainer = cosmosClient.GetContainer(databaseName, userContainerName);
            _questionTypeContainer = cosmosClient.GetContainer(databaseName, questionTypeContainerName);
        }

        public async Task<Core.User> CreateUserAsync(Core.User user)
        {
            var response = await _userContainer.CreateItemAsync(user);
            return response.Resource;
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _userContainer.DeleteItemAsync<Core.User>(userId, new PartitionKey(userId));
        }

        public async Task<Core.User> GetUserByIdAsync(string userId)
        {
            var query = _userContainer.GetItemLinqQueryable<Core.User>()
                .Where(u => u.Id == userId)
                .Take(1)
                .ToFeedIterator();

            var response = await query.ReadNextAsync();
           
         return response.First();
           
        }

        public async Task<Core.User> AddQuestionsAsync(string userId, Core.Question questionsToAdd, List<string> questionsIdsToDelete)
        {
            var query = _userContainer.GetItemLinqQueryable<Core.User>()
              .Where(t => t.Id == userId)
              .Take(1)
              .ToFeedIterator();

            var response = await query.ReadNextAsync();
            var user = response.FirstOrDefault();

            if (user is not null)
            {
        
                if (questionsToAdd != null)
                {
                    user.Questions.Add(questionsToAdd);
                }

             
                if (questionsIdsToDelete != null)
                {
                    user.Questions.RemoveAll(a => questionsIdsToDelete.Contains(a.Id));
                }

                await _userContainer.ReplaceItemAsync(user, user.Id);

              
            }

            return user;
        }

        public async Task<Core.User> UpdateUserAsync(Core.User user)
        {
            var response = await _userContainer.ReplaceItemAsync(user, user.Id);
            return response.Resource;
        }

        public async Task<Core.User> UpdateQuestionsAsync(string userId, string questionId, Core.Question questionsToUpdate)
        {

            var query = _userContainer.GetItemLinqQueryable<Core.User>()
              .Where(t => t.Id == userId)
              .Take(1)
              .ToFeedIterator();

            var response = await query.ReadNextAsync();
            var user = response.FirstOrDefault();

            if (user is not null)
            {
                var question = user.Questions.FirstOrDefault(s => s.Id == questionId);
                if (question != null)
                {
                   question.Text = questionsToUpdate.Text;
                   question.Description = questionsToUpdate.Description;
                  question.Id = questionId;
                }

                

                await _userContainer.ReplaceItemAsync(user, user.Id);


            }

            return user;
        }

        public async Task<QuestionType> CreateQuestionTypeAsync(QuestionType type)
        {
            var response = await _questionTypeContainer.CreateItemAsync(type);
            return response.Resource;
        }


        public async Task<IEnumerable<QuestionType>> GetAllQuestionTypeAsync()
        {
            var query = _questionTypeContainer.GetItemLinqQueryable<QuestionType>()
                .ToFeedIterator();

            var tasks = new List<QuestionType>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                tasks.AddRange(response);
            }

            return tasks;
        }

    }
}
