using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testCosmosdb.Data.Core
{
    public class QuestionType
    {
      

            [JsonProperty("id")]
            public string Id { get; set; } = Guid.NewGuid().ToString();

            [JsonProperty("questionTypeId")]
            public string QuestionTypeId { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }


        
    }
}
