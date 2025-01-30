using QuizApp.Models;

namespace QuizApp.Dtos
{
    public class QuizSearchDto
    {
        public string? SearchTerm { get; set; }
        public CategoryType? Category { get; set; }
        public DifficultyType? Difficulty { get; set; }
        public string SortBy { get; set; } = "newest";
      
    }
}
