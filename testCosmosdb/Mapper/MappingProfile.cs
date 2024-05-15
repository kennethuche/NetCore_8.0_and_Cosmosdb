using AutoMapper;
using testCosmosdb.Data.Core;
using testCosmosdb.ViewModel;
using static testCosmosdb.Data.Core.User;

namespace testCosmosdb.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserVm>().ReverseMap();
            CreateMap<Question, QuestionVm>().ReverseMap();
            CreateMap<QuestionType, QuestionTypeVm>().ReverseMap();
            CreateMap<Data.Core.Program, ProgramVm>().ReverseMap();
            CreateMap<ProgramApplication,ProgramApplicationVm>().ReverseMap();
        
        }
    }
}
