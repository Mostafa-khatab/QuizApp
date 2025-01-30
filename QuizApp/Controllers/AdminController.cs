using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizApp.Dtos;
using QuizApp.Interfaces;
using QuizApp.Models;
using System.Security.Claims;

namespace QuizApp.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminServices _adminServices;

        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [HttpGet("quizzes/pending")]
        public async Task<ActionResult<IEnumerable<QuizReviewDto>>> GetPendingQuizzes()
        {

            try
            {
                return Ok(await _adminServices.GetPendingQuizzes());

            }
            catch(ArgumentException e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPut("quizzes/{id}/status")]
        public async Task<IActionResult> UpdateQuizStatus(int id, [FromBody] QuizStatusUpdateDto dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return StatusCode(StatusCodes.Status500InternalServerError, "User not found");
                await _adminServices.UpdateQuizStatus(id, dto, userId);
                return NoContent();

            }
            catch(ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                return Ok(await _adminServices.GetUsers());

            }
            catch(ArgumentException e) {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("users/{userId}/ban")]
        public async Task<IActionResult> BanUser(string userId, [FromBody] BanRequestDto dto)
        {
            try
            {
                await _adminServices.BanUser(userId, dto);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("users/{userId}/unban")]
        public async Task<IActionResult> UnBanUser(string userId)
        {
            try
            {
                await _adminServices.UnbanUser(userId);
                return Ok();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("reports")]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetReports()
        {
            try
            {
                return Ok(await _adminServices.GetReports());
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }

}
