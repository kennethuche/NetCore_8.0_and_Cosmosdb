using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testCosmosdb.Data.DTO
{
    public class ProgramApplicationDTO
    {

        public string Id { get; set; } 

     
        public string FirstName { get; set; }

      
        public string LastName { get; set; }

        public string Email { get; set; }

    
        public string Phone { get; set; }

        public string Nationality { get; set; }

        public string IdNumber { get; set; }

    
        public DateTimeOffset DateOfBirth { get; set; }

       
        public string Gender { get; set; }

        public string Description { get; set; }
        public string Title { get; set; }
    }
}
