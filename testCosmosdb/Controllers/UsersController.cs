using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using testCosmosdb.Data.Core;
using testCosmosdb.Data.Inteface;
using testCosmosdb.ViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace testCosmosdb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
     
        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ViewModel.UserVm>> GetUser(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<ViewModel.UserVm>(user);
            return Ok(result);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] ViewModel.UserVm userVm)
        {
            var user = _mapper.Map<Data.Core.User>(userVm);
            var createdUser = await _userRepository.CreateUserAsync(user);
            return Created("User Created succefully",createdUser.Id);
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ViewModel.UserVm>> UpdateUser(Guid id, ViewModel.UserVm userVm)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id.ToString());
            if (existingUser == null)
            {
                return NotFound();
            }
            var user = _mapper.Map<Data.Core.User>(userVm);
            user.Id = existingUser.Id; // Preserve the original ID

            var updatedUser = await _userRepository.UpdateUserAsync(user);
            return Ok(updatedUser);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var existingUser = await _userRepository.GetUserByIdAsync(id.ToString());
            if (existingUser == null) return NotFound();
            

            await _userRepository.DeleteUserAsync(id.ToString());
            return NoContent();
        }

        [HttpPut("{userId}/Questions")]
        public async Task<IActionResult> AddQuestions(Guid userId, [FromBody] QuestionVm request)
        {

            var existingUser = await _userRepository.GetUserByIdAsync(userId.ToString());
            if (existingUser == null) return NotFound();

            var question = _mapper.Map<Data.Core.Question>(request);
           
        

            return Ok(await _userRepository.AddQuestionsAsync(userId.ToString(), question, request.QuestionsIdsToDelete.Select(x => x.ToString()).ToList()));
        }

        [HttpPut("{userId}/Questions/{questionId}")]
        public async Task<ActionResult> UpdateSubtaskStatus(Guid userId, Guid questionId,
          [FromBody] QuestionVm request)
        {

            var question = _mapper.Map<Data.Core.Question>(request);
            var updatedTask = await _userRepository.UpdateQuestionsAsync(userId.ToString(), questionId.ToString(), question);
            if (updatedTask == null)
            {
                return NotFound();
            }

            return Ok(updatedTask);
        }

        [HttpPost("questionType")]
        public async Task<IActionResult> CreateQuestionType([FromBody] ViewModel.QuestionTypeVm request)
        {
            var data = _mapper.Map<Data.Core.QuestionType>(request);
            var  result = await _userRepository.CreateQuestionTypeAsync(data);
            return Created("QuestionType Created succefully", data.Id);
        }

        [HttpGet("questionType")]
        public async Task<ActionResult<IEnumerable<QuestionTypeVm>>> GetAllQuestionType()
        {
            var res = await _userRepository.GetAllQuestionTypeAsync();
            if (res == null)
            {
                return NotFound();
            }

            return Ok(res);
        }
    }
}
