using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Entity
{
    public class Answer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
        public int QuestionId { get; set; }
        [ForeignKey(nameof(Answer.QuestionId))]
        public Question Question { get; set; }
    }
}
