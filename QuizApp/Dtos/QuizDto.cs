using QuizApp.Models;

namespace QuizApp.Dtos
{
    public class QuizDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public CategoryType CategoryType { get; set; }
        public DifficultyType DifficultyType { get; set; }
        public int TimeLimitInSecond { get; set; }
    }
}
