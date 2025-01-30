using QuizApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp.Entity
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public List<Answer> Answers { get; set; } = new();
    }
}
