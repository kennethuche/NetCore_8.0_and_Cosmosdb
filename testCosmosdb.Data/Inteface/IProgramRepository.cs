using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testCosmosdb.Data.Core;
using testCosmosdb.Data.DTO;

namespace testCosmosdb.Data.Inteface
{
    public interface IProgramRepository
    {

        Task<Program> CreateProgramAsync(Program program);
        Task<ProgramApplicationDTO> ApplyProgramAsync(ProgramApplication programapplication);
        Task<Program> GetProgramByIdAsync(string programId);

        Task<Core.User> GetUserByIdAsync(string userId);
    }
}
