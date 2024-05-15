using Newtonsoft.Json;

namespace testCosmosdb.ViewModel
{
    public class ProgramVm
    {

    
        public string Title { get; set; }

        public string Description { get; set; }
    }

    public class ProgramApplicationVm
    {
     

  
        public string ProgramId { get; set; }

        public string UserId { get; set; }
    }
}
