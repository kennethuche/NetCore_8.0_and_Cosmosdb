using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testCosmosdb.Data.Core
{
    public class User
    {

         [JsonProperty("id")]
         public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }  
        
        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("idNumber")]
        public string IdNumber { get; set; }

        [JsonProperty("dateOfBirth")]
        public DateTimeOffset DateOfBirth { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

      

        [JsonProperty("questions")]
        public List<Question> Questions { get; set; }
       

        


      
    }


    public class Question
    {

        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("questionTypeId")]
        public int QuestionTypeId { get; set; }
    }
}
