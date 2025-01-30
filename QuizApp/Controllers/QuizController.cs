using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Dtos;
using QuizApp.Entity;
using QuizApp.Interfaces;
using QuizApp.Models;
using QuizApp.Services;
using System.Security.Claims;

namespace QuizApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuizController : ControllerBase
    {
        private readonly IQuizServices _services;

        public QuizController(IQuizServices services)
        {
            _services = services;
        }

        [HttpPost]

        public async Task<IActionResult> CreateQuiz([FromBody] QuizDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated");
                }

                await _services.CreateQuiz(dto , userId);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        } // ok

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateQuiz(int id, [FromBody] QuizDto dto)
        {

            try
            {

                await _services.UpdateQuiz(id, dto);

                return NoContent();

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

        } // ok

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteQuiz(int id)
        {

            try
            {
                await _services.DeleteQuiz(id);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        } // ok

        [HttpGet("GetByCategory/{CateoryType}")]

        public async Task<IActionResult> GetByCategory(CategoryType CateoryType)
        {
            try
            {

                var quiz = await _services.GetQuizByCategory(CateoryType);

                return Ok(quiz);

            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        } // ok

        [HttpGet("GetByDifficulty/{DifficultyType}")]

        public async Task<IActionResult> GetByDifficulty(DifficultyType DifficultyType)
        {
            try
            {

                var quiz = await _services.GetQuizByDificult(DifficultyType);

                return Ok(quiz);

            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        } // ok

        [HttpPost("{quizId}/rate")]

        public async Task<IActionResult> RateQuiz(int quizId, [FromBody] int rating)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("User not authenticated");
                }
                await _services.SubmitRating(quizId, rating, userId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        } // ok

        [HttpGet("leaderboard")]
        public async Task<IActionResult> GetLeaderboard([FromQuery] LeaderboardFilterDto filter)
        {
            try
            {
                var leaderboard = await _services.GetLeaderboard(filter);
                return Ok(leaderboard);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        } // ok

    }
}
