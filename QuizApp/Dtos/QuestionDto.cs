using QuizApp.Entity;
using QuizApp.Models;

namespace QuizApp.Dtos
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public int QuizId { get; set; }
        public List<AnswerDto> Answers { get; set; } = new();
    }
}
