using QuizApp.Dtos;

namespace QuizApp.Interfaces
{
    public interface IAdminServices
    {
        public Task<IEnumerable<QuizReviewDto>> GetPendingQuizzes();
        public Task UpdateQuizStatus(int id, QuizStatusUpdateDto dto, string userId);
        public Task<IEnumerable<UserDto>> GetUsers();
        public Task BanUser(string id, BanRequestDto dto);
        public Task<IEnumerable<ReportDto>> GetReports();
        public Task UnbanUser(string id);
    }
}
