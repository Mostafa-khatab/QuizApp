using Microsoft.AspNetCore.Mvc;
using QuizApp.Dtos;
using QuizApp.Entity;
using QuizApp.Models;

namespace QuizApp.Interfaces
{
    public interface IQuizServices
    {
        public Task CreateQuiz(QuizDto dto , string userID);
        public Task UpdateQuiz(int id, QuizDto dto);
        public Task DeleteQuiz(int id);
        public Task<List<QuizDto>> GetQuizByCategory(CategoryType categoryType);
        public Task<List<QuizDto>> GetQuizByDificult(DifficultyType difficultyType);
        public Task<Quiz> GetQuizWithQuestions(int id);
        public Task IncrementViewCount(int quizId);
        public Task<QuizSession> StartQuizSession(int quizId, string userId);
        public Task<string> SubmitQuizAttempt(QuizAttemptDto attemptDto, string userId);
        public Task<List<QuizDto>> SearchQuizzes(QuizSearchDto searchDto);
        public Task<List<QuizDto>> GetAttemptHistory(string userId);
        public Task SubmitRating(int quizId, int rating, string userId);
        public Task UpdateAverageRating(int quizId);
        public Task<List<LeaderboardEntryDto>> GetLeaderboard(LeaderboardFilterDto filter);
    }
}
