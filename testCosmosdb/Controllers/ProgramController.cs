using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using testCosmosdb.Data.Abstract;
using testCosmosdb.Data.Inteface;

namespace testCosmosdb.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProgramController : Controller
    {

        private readonly IProgramRepository _programRepository;
        private readonly IMapper _mapper;

        public ProgramController(IProgramRepository programRepository, IMapper mapper)
        {
            _programRepository = programRepository;
            _mapper = mapper;
        }
        
        
        [HttpPost]
        public async Task<IActionResult> CreateProgram([FromBody] ViewModel.ProgramVm req)
        {
            var data = _mapper.Map<Data.Core.Program>(req);
            var createdProgram = await _programRepository.CreateProgramAsync(data);
            return Created("Program Created succefully", createdProgram.Id);
        }

        [HttpPost("Apply")]
        public async Task<ActionResult> CreateUser([FromBody] ViewModel.ProgramApplicationVm req)
        {
            var existingUser = await _programRepository.GetUserByIdAsync(req.UserId);
            if (existingUser == null) return NotFound("User Not Found");

            var existingProgram = await _programRepository.GetProgramByIdAsync(req.ProgramId);
            if (existingProgram == null) return NotFound("Program Not Found");


            var data = _mapper.Map<Data.Core.ProgramApplication>(req);
            var result = await _programRepository.ApplyProgramAsync(data);
            return Ok(result);
        }
    }
}
