using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Dtos;
using QuizApp.Interfaces;
using QuizApp.Services;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuizInteractionController : ControllerBase
{
    private readonly IQuizServices _quizServices;
    private readonly IQuestionServices _questionServices;

    public QuizInteractionController(IQuizServices quizServices, IQuestionServices questionServices)
    {
        _quizServices = quizServices;
        _questionServices = questionServices;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchQuizzes([FromQuery] QuizSearchDto searchDto)
    {
        try
        {

            var results = await _quizServices.SearchQuizzes(searchDto);
            return Ok(results);

        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }

    } 

    [HttpPost("start")]
    public async Task<IActionResult> StartQuiz([FromBody] QuizSessionDto dto)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }
            await _quizServices.IncrementViewCount(dto.QuizId);
            var session = await _quizServices.StartQuizSession(dto.QuizId, userId);
            return Ok(session);
        }catch(Exception ex) {
            return BadRequest(ex.Message);
        }
    } 

    [HttpPost("check-answer")]
    public async Task<IActionResult> CheckAnswer([FromBody] AnswerSubmissionDto dto)
    {
        try
        {
            var question = await _questionServices.GetQuestionWithAnswers(dto.QuestionId);
            var isCorrect = question.Answers.ElementAt(dto.SelectedAnswerIndex).IsCorrect;

            return Ok(new
            {
                IsCorrect = isCorrect,
                CorrectAnswer = question.Answers.FirstOrDefault(a => a.IsCorrect)?.Text
            });
        }catch(ArgumentException ex) {
            return BadRequest(ex.Message);
        }
    } 

    [HttpPost("submit-attempt")]
    public async Task<IActionResult> SubmitAttempt([FromBody] QuizAttemptDto dto)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }
            var attempt = await _quizServices.SubmitQuizAttempt(dto, userId);
            return Ok(attempt);
        }catch(ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    } 

    [HttpGet("attempt-history")]
    [Authorize()]
    public async Task<IActionResult> GetAttemptHistory()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized("User not authenticated");
            }
            var history = await _quizServices.GetAttemptHistory(userId);

            return Ok(history);
        }catch(ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    } 
}