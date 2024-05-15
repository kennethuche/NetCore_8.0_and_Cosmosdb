using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using testCosmosdb.Data.Core;
using static testCosmosdb.Data.Core.User;

namespace testCosmosdb.Data.Inteface
{
    public interface IUserRepository
    {

        Task<User> CreateUserAsync(User user);
        Task DeleteUserAsync(string userId);
        Task<User> GetUserByIdAsync(string userId);
        Task<User> UpdateUserAsync(User user);

        Task<User> AddQuestionsAsync(string userId, Question questionsToAdd, List<string> questionsIdsToDelete);
        Task<User> UpdateQuestionsAsync(string userId, string questionId, Question questionsToAdd);

        Task<QuestionType> CreateQuestionTypeAsync(QuestionType type);

        Task<IEnumerable<QuestionType>> GetAllQuestionTypeAsync();
    }
}
