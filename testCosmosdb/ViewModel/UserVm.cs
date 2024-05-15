using Newtonsoft.Json;
using static testCosmosdb.Data.Core.User;

namespace testCosmosdb.ViewModel
{
    public class UserVm
    {
      

    
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

     
        public string Phone { get; set; }


        public string Nationality { get; set; }


        public string IdNumber { get; set; }

  
        public DateTimeOffset DateOfBirth { get; set; }

     
        public string Gender { get; set; }


        public List<QuestionVm> Questions { get; set; }
    }


    public class QuestionVm
    {
    
        public string Text { get; set; }

        public string Description { get; set; }

      
        public int QuestionTypeId { get; set; }
        public List<Guid> QuestionsIdsToDelete { get; set; }
    }
}
