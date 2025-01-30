using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Dtos;
using QuizApp.Interfaces;

namespace QuizApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionServices _questionServices;

        public QuestionsController(IQuestionServices questionServices)
        {
            _questionServices = questionServices;
        }

        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromBody] QuestionDto dto)
        {
            try
            {
                await _questionServices.AddQuestion(dto);
                return Ok(); 
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveQuestion(int id)
        {
            try
            {
                await _questionServices.RemoveQuestion(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuestion(int id, [FromBody] QuestionDto dto)
        {
            try
            {
                await _questionServices.UpdateQuestion(id, dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            
        }
    }
}