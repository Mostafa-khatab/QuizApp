using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizApp.Dtos;
using QuizApp.Interfaces;
using QuizApp.Models;
using System.Security.Claims;

namespace QuizApp.Services
{
    public class AdminServices : IAdminServices
    {

        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AdminServices(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task BanUser(string id, BanRequestDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var result = await _userManager.SetLockoutEndDateAsync(
                user,
                dto.BanDurationDays.HasValue
                    ? DateTimeOffset.UtcNow.AddDays(dto.BanDurationDays.Value)
                    : DateTimeOffset.MaxValue
            );

            if(!result.Succeeded) {
                throw new ArgumentException("Failed to ban user");
            }
            
        }

        public async Task UnbanUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, null);

            if (!result.Succeeded)
            {
                throw new ArgumentException("Failed to unban user");
            }
        }


        public async Task<IEnumerable<QuizReviewDto>> GetPendingQuizzes()
        {
            return await _context.Quizzes
                .Where(q => q.Status == QuizStatus.Pending)
                .Include(q => q.User)
                .Select(q => new QuizReviewDto
                {
                    Id = q.Id,
                    Title = q.Title,
                    CreatorName = q.User.UserName,
                    CreatedAt = q.CreatedAt,
                    QuestionCount = q.Questions.Count,
                    ReportCount = q.Reports.Count(r => r.Status == ReportStatus.Open)
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ReportDto>> GetReports()
        {
            return await _context.Reports
               .Include(r => r.Quiz)
               .Include(r => r.Reporter)
               .Where(r => r.Status == ReportStatus.Open)
               .Select(r => new ReportDto
               {
                   Id = r.Id,
                   QuizId = r.QuizId,
                   QuizTitle = r.Quiz.Title,
                   Reason = r.Reason,
                   ReportedAt = r.ReportedAt,
                   ReporterName = r.Reporter.UserName
               })
               .ToListAsync();
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            var users = await _userManager.Users
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserName = u.UserName,
                    IsBanned = u.LockoutEnd > DateTimeOffset.Now
                })
                .ToListAsync(); 

            foreach (var user in users)
            {
                var appUser = await _userManager.FindByIdAsync(user.Id);
                user.Roles = await _userManager.GetRolesAsync(appUser);
            }

            return users;
        }


        public async Task UpdateQuizStatus(int id, QuizStatusUpdateDto dto , string userId)
        {
            var quiz = await _context.Quizzes
                .Include(q => q.User)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (quiz == null)
            {
                throw new ArgumentException("Quiz not found");
            }

            quiz.Status = dto.Approved ? QuizStatus.Approved : QuizStatus.Rejected;
            quiz.ReviewedAt = DateTime.UtcNow;
            quiz.ReviewerId = userId;
            quiz.RejectionReason = dto.Reason;

            await _context.SaveChangesAsync();
            
        }
    }
}
