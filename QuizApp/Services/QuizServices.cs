using Microsoft.EntityFrameworkCore;
using QuizApp.Dtos;
using QuizApp.Entity;
using QuizApp.Interfaces;
using QuizApp.Models;
using System.Security.Claims;

namespace QuizApp.Services
{
    public class QuizServices : IQuizServices
    {

        private readonly AppDbContext _context;

        public QuizServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateQuiz(QuizDto dto , string userId)
        {
            var quiz = new Quiz
            {
                Title = dto.Title,
                Description = dto.Description,
                CategoryType = dto.CategoryType,
                DifficultyType = dto.DifficultyType,
                TimeLimitSeconds = dto.TimeLimitInSecond,
                UserId = userId
            };

            await _context.Quizzes.AddAsync(quiz);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateQuiz(int id, QuizDto dto)
        {
            var quiz = _context.Quizzes.FirstOrDefault(x => x.Id == id);

            if (quiz == null)
            {
                throw new ArgumentException("Quiz Not Found");
            }

            quiz.Title = dto.Title;
            quiz.Description = dto.Description;
            quiz.CategoryType = dto.CategoryType;
            quiz.DifficultyType = dto.DifficultyType;
            quiz.TimeLimitSeconds = dto.TimeLimitInSecond;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuiz(int id)
        {
            var quiz = await _context.Quizzes.FirstOrDefaultAsync(x => x.Id == id);

            if (quiz == null)
            {
                throw new ArgumentException("Quiz Not Found");
            }

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
        }

        public async Task<List<QuizDto>> GetQuizByCategory(CategoryType categoryType)
        {
           var quiz = await _context.Quizzes.Where(x => x.CategoryType == categoryType).ToListAsync();

            return quiz.Select(x => new QuizDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                CategoryType = x.CategoryType,
                DifficultyType = x.DifficultyType,
                TimeLimitInSecond = x.TimeLimitSeconds
            }).ToList();

        }

        public async Task<List<QuizDto>> GetQuizByDificult(DifficultyType difficultyType)
        {

            var quiz = await _context.Quizzes.Where(x => x.DifficultyType == difficultyType).ToListAsync();

            return quiz.Select(x => new QuizDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                CategoryType = x.CategoryType,
                DifficultyType = x.DifficultyType,
                TimeLimitInSecond = x.TimeLimitSeconds
            }).ToList();

        }

        public async Task<Quiz> GetQuizWithQuestions(int id)
        {

            return await _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task IncrementViewCount(int quizId)
        {
            var quiz = await _context.Quizzes.FindAsync(quizId);
            if (quiz != null)
            {
                quiz.ViewCount++;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<QuizSession> StartQuizSession(int quizId, string userId)
        {

            var quiz = await _context.Quizzes.FirstOrDefaultAsync(e => e.Id == quizId);

            if (quiz == null)
            {
                throw new ArgumentException("Quiz Not Found");
            }

            var session = new QuizSession
            {
                UserId = userId,
                QuizId = quizId,
                StartTime = DateTime.UtcNow,
                TimeRemaining = (await _context.Quizzes.FindAsync(quizId)).TimeLimitSeconds,
                IsCompleted = false
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<string> SubmitQuizAttempt(QuizAttemptDto attemptDto, string userId) 
        {
            var quizExist = await _context.Quizzes.FirstOrDefaultAsync(q => q.Id == attemptDto.QuizId);
            if(quizExist == null)
            {
                throw new ArgumentException("Quiz Not Found");
            }
            var sessionExist = await _context.Sessions.FirstOrDefaultAsync(s => s.Id == attemptDto.SessionId);

            if(sessionExist == null)
            {
                throw new ArgumentException("Session Not Found");
            }
            var quiz = await GetQuizWithQuestions(attemptDto.QuizId);
            var session = await _context.Sessions.FindAsync(attemptDto.SessionId);

            var elapsed = DateTime.UtcNow - session.StartTime;
            if (elapsed.TotalSeconds > quiz.TimeLimitSeconds)
            {
                session.IsCompleted = true;
                await _context.SaveChangesAsync();
                throw new ArgumentException("Time limit exceeded");
            }

            var correctAnswers = attemptDto.Answers
                .Count(a => quiz.Questions
                    .Any(q => q.Id == a.QuestionId &&
                            q.Answers.ElementAt(a.SelectedAnswerIndex).IsCorrect));

            var attempt = new QuizAttempt
            {
                UserId = userId,
                QuizId = attemptDto.QuizId,
                Score = correctAnswers,
                TotalQuestions = quiz.Questions.Count,
                AttemptDate = DateTime.UtcNow,
                Duration = elapsed
            };

            _context.Attempts.Add(attempt);
            await _context.SaveChangesAsync();
            var percentageScore = (int)Math.Round((correctAnswers / (double)quiz.Questions.Count) * 100);
            return $"{ percentageScore}%";
        }

        public async Task<List<QuizDto>> SearchQuizzes(QuizSearchDto searchDto)
        {
            var query = _context.Quizzes.AsQueryable();

            if (!string.IsNullOrEmpty(searchDto.SearchTerm))
                query = query.Where(q => q.Title.Contains(searchDto.SearchTerm));

            if (searchDto.Category.HasValue)
                query = query.Where(q => q.CategoryType == searchDto.Category);

            if (searchDto.Difficulty.HasValue)
                query = query.Where(q => q.DifficultyType == searchDto.Difficulty);

            query = searchDto.SortBy.ToLower() switch
            {
                "popular" => query.OrderByDescending(q => q.ViewCount),
                "highest-rated" => query.OrderByDescending(q => q.AverageRating),
                _ => query.OrderByDescending(q => q.CreatedAt)
            };

            return await query.Select(q => new QuizDto
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description,
                CategoryType = q.CategoryType,
                DifficultyType = q.DifficultyType,
                TimeLimitInSecond = q.TimeLimitSeconds
            }).ToListAsync();
        }

        public async Task<List<QuizDto>> GetAttemptHistory(string userId)
        {
            var history = await _context.Attempts
            .Where(a => a.UserId == userId)
            .Include(a => a.Quiz)
            .ThenInclude(Quiz => Quiz.Questions)
            .ThenInclude(q => q.Answers)
            .OrderByDescending(a => a.AttemptDate)
            .ToListAsync();

            return history.Select(a => new QuizDto
            {
                Id = a.Quiz.Id,
                Title = a.Quiz.Title,
                CategoryType = a.Quiz.CategoryType,
                DifficultyType = a.Quiz.DifficultyType,
                TimeLimitInSecond = a.Quiz.TimeLimitSeconds
            }).ToList();
        } 

        public async Task SubmitRating(int quizId, int rating, string userId)
        {
            var quiz = await _context.Quizzes.FirstOrDefaultAsync(e => e.Id == quizId);

            if(quiz == null)
            {
                throw new ArgumentException("Quiz Not Found");
            }
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5");

            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.QuizId == quizId && r.UserId == userId);

            if (existingRating != null)
            {
                existingRating.Rating = rating;
            }
            else
            {
                _context.Ratings.Add(new QuizRating
                {
                    QuizId = quizId,
                    UserId = userId,
                    Rating = rating
                });
            }

            await _context.SaveChangesAsync();

            await UpdateAverageRating(quizId);
        } 

        public async Task UpdateAverageRating(int quizId)
        {
            var average = await _context.Ratings
                .Where(r => r.QuizId == quizId)
                .AverageAsync(r => (double?)r.Rating) ?? 0;

            var quiz = await _context.Quizzes.FindAsync(quizId);
            quiz.AverageRating = Math.Round(average, 1);
            await _context.SaveChangesAsync();
        } 

        public async Task<List<LeaderboardEntryDto>> GetLeaderboard(LeaderboardFilterDto filter)
        {

            var quiz = await _context.Quizzes.FirstOrDefaultAsync(e => e.Id == filter.QuizId);

            if(quiz == null)
            {
                throw new ArgumentException("Quiz Not Found");
            }

            if(filter.TopN < 1)
            {
                throw new ArgumentException("Write Positive Integer Only");
            }

            var query = _context.Attempts
                .Include(a => a.User)
                .Include(a => a.Quiz)
                .ThenInclude(a => a.Questions)
                .Where(a => a.UserId != null)  
                .AsQueryable();

            if (filter.QuizId.HasValue)
            {
                query = query.Where(a => a.QuizId == filter.QuizId);
            }

            if (filter.TimeRange.HasValue)
            {
                var cutoffDate = DateTime.UtcNow - filter.TimeRange.Value;
                query = query.Where(a => a.AttemptDate >= cutoffDate);
            }

            return await query
                .GroupBy(a => a.UserId)
                .Select(g => new LeaderboardEntryDto
                {
                    Username = g.First().User.UserName ?? "Anonymous",
                    Score = g.Max(a => a.Score),
                    TotalQuestions = g.First().Quiz.Questions.Count,
                    Percentage = (double)g.Max(a => a.Score) / g.First().Quiz.Questions.Count * 100,
                    AttemptDate = g.Max(a => a.AttemptDate)
                })
                .OrderByDescending(e => e.Score)
                .ThenBy(e => e.AttemptDate)
                .Take(filter.TopN)
                .ToListAsync();
        } 
    }
}
